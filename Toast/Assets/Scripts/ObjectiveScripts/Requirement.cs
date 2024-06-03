using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

[Serializable]
public class Requirement
{
    // ------------------------------- Variables -------------------------------

    [Header("Requirement Type and Attribute Flags")]
    public RequirementType type = RequirementType.EatObject;
    public PropFlags targetAttributes;

    [Header("Goal Information")]
    public string goalName;
    public int goal;
    public bool alwaysListening = false;

    [SerializeField, Header("Event Listeners")]
    private PropIntEventListenerGroup listeners = new PropIntEventListenerGroup();

    // Private variables
    private int current;
    private bool complete = false;
    private bool completeOnNextFrame = false;

    [NaughtyAttributes.HorizontalLine, Header("Other Variables")]
    // ID TRACKER DO NOT CHANGE
    public int id = -1;
    // Public variables
    public bool listening = false;


    // ------------------------------- Properties -------------------------------
    public int Current { get => current; set => current = value; }
    public int ID { get => id; set => id = value; }

    /// <summary>
    /// Checks if this goal is complete
    /// </summary>
    /// <returns></returns>
    public bool CheckComplete()
    {
        if(goal > 0) // If the goal is a completable goal
        {
            if(current == goal) // Exact match
            {
                if (!complete) // If it was not complete, run one-shot effects
                {
                    completeOnNextFrame = true;
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
    public void UpdateRequirement(RequirementEvent e)
    {
        // If listening and correct type and incomplete
        if((listening || alwaysListening) && e.type == type && !complete) // Ensure type, target, and listening
        {
            // If does not contain all necessary flags, return
            if(!(e.attributes.HasFlag(targetAttributes)))
            {
                return;
            }
            
            if (e.increase) // Increase or Decrease
            {
                current += 1;
            }
            else
            {
                current = current > 0 ? current - 1 : 0;
            }

            if(type != RequirementType.HaveObject) // If not [exact number type], don't allow overflow
            {
                if(goal > 0)
                {
                    if(current > goal)
                    {
                        current = goal;
                    }
                }
            }

            // Specific case for handling Toast Ninja score
            if(type == RequirementType.ToastNinjaScore)
            {
                // Undo previous increase
                current -= 1;

                // Check for Jam, Toast, Bread
                if (e.attributes.HasFlag(PropFlags.Jam))
                {
                    current += 2;
                }
                else if (e.attributes.HasFlag(PropFlags.Toast))
                {
                    current -= 2;
                }
                else
                {
                    current += 1;
                }

                // Reset if not increase
                if (!e.increase)
                {
                    current = 0;
                }
            }
        }
    }

    /// <summary>
    /// Forces this requirement to complete
    /// </summary>
    public void ForceComplete()
    {
        current = goal;
    }

    /// <summary>
    /// Returns a formatted string to be displayed to the player
    /// </summary>
    new public string ToString
    {
        get
        {
            string value = "";
            if (CheckComplete())
            {
                value += "<color=#111><s><size=-2>";
                value += goalName + " ";
                value += "DONE!";
            }
            else
            {
                if(goal > 0)
                {
                    value += "<color=#000><size=+0>";
                    value += goalName + " ";
                    value += current + "/" + goal;
                }
                else
                {
                    value += "<color=#000><size=+0>";
                    value += goalName + " ";
                    value += current;
                }
                
            }
            value += "</size></s></color>";
            return value;
        }
    }
}
