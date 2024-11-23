/*
 * Detect Prop script - Author: Nick Kannenberg
 * 
 * This script is part of the puzzles utilities story
 * It is used to detect props within a given volume, then trigger set events when those props meet set requirements
 * This script has two modes:
 * Non-Cumulative
 *      Checks for a specific number of props in the area, each with the specified attributes
 * 
 * Cumulative
 *      Checks for an (optional) specific number of props in the area,
 *      with the cumulative attributes of all props in the area equaling the specified list of attributes 
 */

using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DetectItem : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Prop Variables")]
    [SerializeField] // List of attributes to chekc for
    private PropFlags attributes;
    [SerializeField] // Is this cumulative
    private bool flagsAreCumulative = false;
    private PropFlags cumulativeFlags;
    [SerializeField]
    private bool showRequiredFlags = true;
    [SerializeField]
    private bool showItemsCollectedHeader = true;

    [Header("Completion Variables")]
    [SerializeField] // Number of items to check for
    private int numItems;
    private int curItems;
    private bool itemReqsMet = false;
    [SerializeField] // Does it need to be an exact number of items
    private bool exactValue = false;
    [SerializeField] // Amount of time items need to stay to complete goal (0 for instant)
    private float itemStayTime;
    private float cStayTime;
    [SerializeField] // Text displaying the reward
    private string rewardText;
    [SerializeField]
    private string goalText;
    [SerializeField] // Destroy items in volume when complete?
    private bool destroyItemsOnComplete = false;
    [SerializeField] // Destroy this when complete?
    private bool destroyOnComplete = false;
    [SerializeField] // Reference to physical display UI
    private TextMeshPro displayReference;

    private bool complete = false;

    [Header("Unity Events")]
    [SerializeField] // Event run when this is complete
    private UnityEvent completionEvent;

    [SerializeField] // Does this have an event on start?
    private bool eventOnStart;
    [ShowIf("eventOnStart")]
    [SerializeField] // Event run when first object is placed in here
    private UnityEvent startEvent;
    private bool interacted = false;

    [SerializeField] // Does this have an event at specific progress?
    private bool eventOnProgress;
    [ShowIf("eventOnProgress")]
    [SerializeField] // Progress value to trigger event at
    private int progressValue;
    private bool progressTriggered = false;
    [ShowIf("eventOnProgress")]
    [SerializeField] // Event that is triggered upon reaching that progress
    private UnityEvent progressEvent;

    [Header("Requirement Events")]
    [SerializeField] // Event for completing 
    private PropIntGameEvent rCompleteEvent;
    [ShowIf("eventOnStart")]
    [SerializeField] // Event for starting
    private PropIntGameEvent rStartEvent;
    [ShowIf("eventOnProgress")]
    [SerializeField] // Event for hitting progress
    private PropIntGameEvent rProgressEvent;

    private List<NewProp> propsInTrigger = new List<NewProp>();

    // ------------------------------- Functions -------------------------------
    // On start, set stay time and display text
    void Start()
    {
        cStayTime = itemStayTime;
        UpdateDisplayText();
    }

    // On update, only if Reqs met, check stay time for completion and decrement time
    void Update()
    {
        if (itemReqsMet)
        {
            if (cStayTime <= 0)
            {
                Complete();
            }
            else
            {
                cStayTime -= Time.deltaTime;
                UpdateDisplayText();
            }
        }
    }

    /// <summary>
    /// On entering the trigger, check if it has prop, then pass
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        NewProp otherProp = other.GetComponent<NewProp>();
        if (otherProp != null)
        {
            PropEnterTrigger(otherProp);
        }
    }

    /// <summary>
    /// On exiting trigger, check if it has prop, then pass
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        NewProp otherProp = other.GetComponent<NewProp>();
        if (otherProp != null)
        {
            PropExitTrigger(otherProp);
        }
    }

    /// <summary>
    /// When a prop enters trigger, run checks on its attributes and add it to list
    /// </summary>
    /// <param name="prop">The prop entering the trigger</param>
    private void PropEnterTrigger(NewProp prop)
    {
        // If exact value, count all things that enter
        if (exactValue)
        {
            curItems += 1;
        }

        // Run attribtue checks
        if (prop.HasFlag(attributes) || flagsAreCumulative)
        {
            if (!propsInTrigger.Contains(prop))
            {
                // Add to list
                propsInTrigger.Add(prop);

                // If not yet interacted, trigger oneshots
                if (!interacted)
                {
                    interacted = true;
                    startEvent.Invoke();
                    if(rStartEvent != null)
                        rStartEvent.RaiseEvent(prop, 1);
                }

                // Only add if exactValue didnt already add
                if (!exactValue)
                {
                    curItems += 1;
                }

                // Check progress events and trigger oneshots
                if(curItems >= progressValue && !progressTriggered)
                {
                    progressEvent.Invoke();
                    if (rProgressEvent != null)
                        rProgressEvent.RaiseEvent(prop, 1);
                    progressTriggered = true;
                }
            }

            // If cumulative, run calcs
            if (flagsAreCumulative)
            {
                CalculateCumulativePropFlags();
            }
        }

        CompletionCheck();
    }

    /// <summary>
    /// When a prop exts the trigger, check if it was stored, then run checks
    /// </summary>
    /// <param name="prop"></param>
    private void PropExitTrigger(NewProp prop)
    {
        // If exact value, count all things that exit
        if (exactValue)
        {
            curItems -= 1;
        }

        // If it was contained, then remove it
        if (propsInTrigger.Contains(prop))
        {
            propsInTrigger.Remove(prop);
            // If exact value, dont double count
            if (!exactValue)
            {
                curItems -= 1;
            }

            // If cumulative, run calcs
            if (flagsAreCumulative)
            {
                CalculateCumulativePropFlags();
            }
        }

        CompletionCheck();
    }

    /// <summary>
    /// Calculates cumulative prop flags of all items currently stored
    /// </summary>
    private void CalculateCumulativePropFlags()
    {
        cumulativeFlags = PropFlags.None;
        foreach (NewProp prop in propsInTrigger)
        {
            cumulativeFlags |= prop.propFlags;
        }
    }

    /// <summary>
    /// Checks if this goal is complete
    /// </summary>
    private void CompletionCheck()
    {
        if(((curItems >= numItems && !exactValue) || (curItems == numItems && exactValue)) && !flagsAreCumulative)
        {
            itemReqsMet = true;
        }
        else
        {
            itemReqsMet = false;
            cStayTime = itemStayTime;
        }

        if (flagsAreCumulative)
        {
            if (cumulativeFlags.HasFlag(attributes) && ((curItems >= numItems && !exactValue) || (curItems == numItems && exactValue)))
            {
                itemReqsMet = true;
            }
            else
            {
                itemReqsMet = false;
                cStayTime = itemStayTime;
            }
        }

        UpdateDisplayText();
    }

    /// <summary>
    /// Runs completion effects
    /// </summary>
    private void Complete()
    {
        completionEvent.Invoke();
        if(rCompleteEvent != null)
            rCompleteEvent.RaiseEvent(propsInTrigger[0], 1);
        complete = true;

        UpdateDisplayText();

        if (destroyItemsOnComplete)
        {
            foreach (NewProp prop in propsInTrigger)
            {
                Destroy(prop.gameObject);
            }
        }

        if (destroyOnComplete)
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Override ToString, returns all info for this detector
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string value = "";
        if (complete)
        {
            value += "<b>Complete!</b>";
            if(rewardText.Length > 0)
            {
                value += "\n\n" + rewardText;
            }
            return value;
        }

        if (flagsAreCumulative)
        {
            // Target num objects, none if 1 and !exactValue, strikethrough otherwise if complete
            if (exactValue)
            {
                if (curItems == numItems || numItems <= 0)
                {

                }
                else
                {
                    value += $"<u>Items Collected</u>: <b>{curItems}</b> of <b>{numItems}</b>\n<u></u>"; 
                }
            }
            else
            {
                if(numItems <= 1)
                {
                    value += "<u></u>";
                }
                else if(curItems >= numItems)
                {
                    value += "<u>All Items Collected</u><u></u>\n";
                }
                else
                {
                    value += $"<u>Items Collected:</u> <b>{curItems}</b> of <b>{numItems}</b><u></u>\n";
                }
            }

            if (showRequiredFlags)
            {
                // Target flags, strikethrough if complete
                foreach(PropFlags f in Enum.GetValues(typeof(PropFlags)))
                {
                    if (attributes.HasFlag(f) && !PropFlags.None.Equals(f))
                    {
                        if (cumulativeFlags.HasFlag(f))
                        {
                            value += $"<s><color=#111>{f}</color></s>\n";
                        }
                        else
                        {
                            value += $"{f}\n";
                        }
                    }
                }
            }

            // Reward
            if (rewardText.Length > 0)
            {
                value += $"\n{rewardText}";
            }
        }
        else
        {
            if(goalText.Length > 0)
            {
                value += $"{goalText}\n\n";
            }
            if (showItemsCollectedHeader)
            {
                // Number collected over total number
                value += $"<u>Items Collected:</u>";
            }

            value +=  $"<b>{curItems}</b> of <b>{numItems}</b>\n";

            if (showRequiredFlags)
            {
                // Target attributes of objects
                value += "<u></u> \n";
                foreach (PropFlags f in Enum.GetValues(typeof(PropFlags)))
                {
                    if (attributes.HasFlag(f) && !PropFlags.None.Equals(f))
                    {
                        value += $"{f}\n";   
                    }
                }
            }

            // Reward
            if(rewardText.Length > 0)
            {
                value += $"\n{rewardText}";
            }
        }

        if (itemReqsMet)
        {
            value = "<b>Hold Still!<b> \n\n";
            string t = cStayTime.ToString();
            t = t.PadRight(4, '0').Substring(0, 4);
            value += t;
        }

        return value;
    }

    private void UpdateDisplayText()
    {
        if(displayReference != null)
        {
            displayReference.text = this.ToString();
        }
    }

    private void OnDrawGizmosSelected()
    {
        UpdateDisplayText();   
    }
}
