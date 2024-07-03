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
            if(current == goal) // Exact match
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
        // If listening and correct type and incomplete
        if((listening || alwaysListening) && !complete) // Ensure type, target, and listening
        {
            // If does not contain all necessary flags, return
            if(!(e.attributes.HasFlag(targetAttributes)))
            {
                return;
            }

            if(hasExclusions && e.attributes.HasFlag(excludeAttributes))
            {
                return;
            }

            current += value;
        
            if(!exactValueGoal) // If not [exact number type], don't allow overflow
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
                if (CheckComplete())
                {
                    value += "<size=-6><color=#333> - <s>";
                    value += goalName + " ";
                    //value += "DONE!";
                }
                else
                {
                    if(goal > 0)
                    {
                        value += "<color=#000> - ";
                        value += goalName + " ";
                        value += "<pos=80%>";
                        value += current + "/" + goal;
                    }
                    else
                    {
                        value += "<color=#000> - ";
                        value += goalName + " ";
                        value += "<pos=80%>";
                        value += current;
                    }
                    
                }
                value += "</size></s></color>";
            }
            return value;
        }
    }
}
