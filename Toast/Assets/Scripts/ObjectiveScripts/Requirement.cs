using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class Requirement : MonoBehaviour
{
    // Public variables
    public bool listening = false;
    public RequirementType type = RequirementType.EatObject;
    public PropFlags targetAttributes;
    public string goalName;
    public int goal;
    public bool alwaysListening = false;
    private bool complete = false;
    private bool completeOnNextFrame = false;

    // Private variables
    private int current;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (completeOnNextFrame && !complete)
        {
            complete = true;
            AudioManager.instance.PlayOneShotSound(AudioManager.instance.requirementComplete);
        }
    }

    // Check if the goal is complete
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

    // Update the requirement with a given event
    public void UpdateRequirement(RequirementEvent e)
    {
        // If listening and correct type
        if((listening || alwaysListening) && e.type == type) // Ensure type, target, and listening
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

            if(type != RequirementType.HaveObject && type != RequirementType.ToastNinjaScore) // If not [exact number type], don't allow overflow
            {
                if(current > goal)
                {
                    current = goal;
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

    // Returns a formatted string of this requirement to be displayed to the player
    new public string ToString
    {
        get
        {
            string value = "";
            if (CheckComplete())
            {
                value += "<color=#111><s><size=-2>";
                value += goalName + ": ";
                value += "DONE!";
            }
            else
            {
                if(goal > 0)
                {
                    value += "<color=#000><size=+0>";
                    value += goalName + ": ";
                    value += current + "/" + goal;
                }
                else
                {
                    value += "<color=#000><size=+0>";
                    value += goalName + ": ";
                    value += current;
                }
                
            }
            value += "</size></s></color>";
            return value;
        }
    }
}
