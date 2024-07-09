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
    [SerializeField]
    private PropFlags attributes;
    [SerializeField]
    private bool flagsAreCumulative = false;
    private PropFlags cumulativeFlags;

    [Header("Completion Variables")]
    [SerializeField]
    private int numItems;
    private int curItems;
    private bool itemReqsMet = false;
    [SerializeField]
    private bool exactValue = false;
    [SerializeField]
    private float itemStayTime;
    private float cStayTime;
    [SerializeField]
    private string rewardText;
    [SerializeField]
    private bool destroyItemsOnComplete = false;
    [SerializeField]
    private bool destroyOnComplete = false;
    [SerializeField]
    private TextMeshPro displayReference;

    private bool complete = false;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent completionEvent;

    [SerializeField]
    private bool eventOnStart;
    [ShowIf("eventOnStart")]
    [SerializeField]
    private UnityEvent startEvent;
    [ShowIf("eventOnStart")]
    [SerializeField]
    private bool interacted = false;

    [SerializeField]
    private bool eventOnProgress;
    [ShowIf("eventOnProgress")]
    [SerializeField]
    private int progressValue;
    private bool progressTriggered = false;
    [ShowIf("eventOnProgress")]
    [SerializeField]
    private UnityEvent progressEvent;

    [Header("Requirement Events")]
    [SerializeField]
    private PropIntGameEvent rCompleteEvent;
    [ShowIf("eventOnStart")]
    [SerializeField]
    private PropIntGameEvent rStartEvent;
    [ShowIf("eventOnProgress")]
    [SerializeField]
    private PropIntGameEvent rProgressEvent;

    private List<NewProp> propsInTrigger = new List<NewProp>();

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        cStayTime = itemStayTime;
        UpdateDisplayText();
    }

    // Update is called once per frame
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
                if (curItems == numItems)
                {

                }
                else
                {
                    value += $"<u>Items Collected</u>: <b>{curItems}</b> of <b>{numItems}</b>\n<u>Target Attributes:</u>\n"; 
                }
            }
            else
            {
                if(numItems <= 1)
                {
                    value += "<u>Target Attributes:</u> \n";
                }
                else if(curItems >= numItems)
                {
                    value += "<u>All Items Collected</u>\n<u>Target Attributes:</u>\n";
                }
                else
                {
                    value += $"<u>Items Collected:</u> <b>{curItems}</b> of <b>{numItems}</b>\n<u>Target Attributes:</u>\n";
                }
            }

            // Target flags, strikethrough if complete
            foreach(PropFlags f in Enum.GetValues(typeof(PropFlags)))
            {
                if (attributes.HasFlag(f) && !PropFlags.None.Equals(f))
                {
                    if (cumulativeFlags.HasFlag(f))
                    {
                        value += $"<s>{f}</s>\n";
                    }
                    else
                    {
                        value += $"{f}\n";
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
            // Number collected over total number
            value += $"<u>Items Collected:</u> <b>{curItems}</b> of <b>{numItems}</b>\n";

            // Target attributes of objects
            value += "\n<u>Target Attributes:</u> \n";
            foreach (PropFlags f in Enum.GetValues(typeof(PropFlags)))
            {
                if (attributes.HasFlag(f) && !PropFlags.None.Equals(f))
                {
                    value += $"{f}\n";   
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
