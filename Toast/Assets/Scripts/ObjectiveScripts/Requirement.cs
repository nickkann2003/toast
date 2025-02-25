using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Requirement
{
    // ------------------------------- Variables -------------------------------

    [Header("Attribute Flags and Event Listener")]
    //public RequirementType type = RequirementType.EatObject;
    public PropFlags targetAttributes;
    public bool hasExclusions = false;
    [SerializeField, ShowIf("hasExclusions")]
    private PropFlags excludeAttributes;

    public List<PropAttributeSO> targetPropAttributes;
    public List<PropAttributeSO> excludePropAttributes;

    [SerializeField, Label("Do Not Add Response Event")]
    private PropIntGameEventListener listener = new PropIntGameEventListener();
    // Used to mark if this objective was completed by a load, ensures OneShot effects play at the correct time
    private bool loadCompleted = false;

    [Header("Goal Information")]
    public string goalName;
    public int goal;
    public bool exactValueGoal = false;
    public bool alwaysListening = false;

    // Private variables
    private int current;
    private bool complete = false;

    [NaughtyAttributes.HorizontalLine, Header("Other Variables")]
    // ID TRACKER DO NOT CHANGE
    public int id = -1;
    // Public variables
    public bool listening = false;

    public bool isHighScore = false;
    public bool isCurrentScore = false;


    // ------------------------------- Properties -------------------------------
    public int Current { get => current; set => current = value; }
    public int ID { get => id; set => id = value; }
    public bool Complete { get => complete; set => complete = value; }


    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Runs when this requirement is loaded
    /// </summary>
    public void OnLoad()
    {
        // Reset values on game start
        current = 0;
        complete = false;
        listening = false;

        // Save event type
        PropIntGameEvent e = listener.GameEvent;
        // Recreate listener (issue with SO)
        listener = new PropIntGameEventListener();
        listener.GameEvent = e;
        // Remove all listeners
        listener.Response.RemoveAllListeners();
        // Enable it
        listener.OnEnable();
        // Add function to listener
        listener.Response.AddListener(UpdateRequirement);
    }

    /// <summary>
    /// Checks if this goal is complete
    /// </summary>
    /// <returns></returns>
    public bool CheckComplete()
    {
        if(goal > 0) // If the goal is a completable goal
        {
            if(current >= goal) // Exact match
            {
                if (!complete || loadCompleted) // If it was not complete, run one-shot effects
                {
                    complete = true;
                    loadCompleted = false;
                    AudioManager.instance.PlayOneShotSound(AudioManager.instance.requirementComplete);
                }
                return true;
            }
            return false;
        }
        return false;
    }
    
    /// <summary>
    /// Updates this requirement with an event
    /// </summary>
    /// <param name="e"></param>
    public void UpdateRequirement(NewProp e, int value)
    {
        // If e is null (Only case is toast ninja), run toast ninja checks
        if(e == null)
        {
            if (isHighScore)
            {
                if (value >= current)
                {
                    current = value;
                }
            }
            else if (isCurrentScore)
            {
                current = value;
            }
            else
            {
                // Was a null triggering event
                current += value;
            }

            ObjectiveManager.instance.UpdateText();
            return;
        }

        // If listening and correct type and incomplete
        if((listening || alwaysListening) && !complete) // Ensure type, target, and listening
        {
            // If does not contain all necessary flags, return
            if(!e.HasAttributes(targetPropAttributes))
            {
                return;
            }

            if(hasExclusions && e.HasAnyAttribute(excludePropAttributes))
            {
                return;
            }

            if (isHighScore)
            {
                if(value >= current)
                {
                    current = value;
                }
            }else if (isCurrentScore)
            {
                current = value;
            }
            else
            {
                current += value;
            }

            if (!exactValueGoal) // If not [exact number type], don't allow overflow
            {
                if(goal > 0)
                {
                    if(current > goal)
                    {
                        current = goal;
                    }
                }
            }
            ObjectiveManager.instance.UpdateText();
        }
    }

    /// <summary>
    /// Forces this requirement to complete
    /// </summary>
    public void ForceComplete()
    {
        current = goal;
        complete = true;
        loadCompleted = true;
        listening = true;
    }

    public void OnDisable()
    {
        listener.OnDisable();
    }

    /// <summary>
    /// Returns a formatted string to be displayed to the player
    /// </summary>
    new public string ToString
    {
        get
        {
            string value = "";
            if (listening)
            {
                if (complete || CheckComplete())
                {
                    value += "<size=-6><color=#333><s>";
                    value += goalName + " ";
                    //value += "DONE!";
                }
                else
                {
                    if(goal > 1)
                    {
                        value += "<color=#000>";
                        value += goalName + "   ";
                        value += current + "/" + goal;
                    }
                    else if(goal <= 0)
                    {
                        value += "<color=#000>";
                        value += goalName + "   ";
                        value += current;
                    }else if(goal == 1)
                    {
                        value += "<color=#000>";
                        value += goalName + " ";
                    }
                    
                }
                value += "</size></s></color>";
            }
            return value;
        }
    }
}
