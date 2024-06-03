using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;


//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Objective
{
    // ------------------------------- Variables -------------------------------
    // Public variables
    [Header("Objective Scriptable Object")]
    [SerializeField]
    private SO_Objective objectiveInfo;

    [Header("Pre-Requisite Objectives")]
    public List<Objective> prerequisites = new List<Objective>();

    [Header("Activatables Upon Completion")]
    public List<GameObject> activatables = new List<GameObject>();

    [Header("Unity Events")]
    public UnityEvent completionEvents;

    // ------------------------------- Properties -------------------------------
    public bool Complete { get => objectiveInfo.Complete; }
    public int ID { get => objectiveInfo.ID; }

    // ------------------------------- Functions -------------------------------
    // Check if the current task has had its prerequisites complete
    public bool CheckAvailable()
    {
        if (prerequisites.Count > 0)
        {
            foreach (Objective obj in prerequisites)
            {
                if (!obj.Complete)
                {
                    objectiveInfo.Available = false;
                    return false;
                }
            }
        }
        objectiveInfo.Available = true;
        return true;
    }

    // Check if this objective is complete
    public bool CheckComplete()
    {
        if (!objectiveInfo.Complete)
        {
            foreach (Requirement r in requirements)
            {
                if (!r.CheckComplete())
                {
                    complete = false;
                    return false;
                }
            }
            foreach (GameObject ob in activatables)
            {
                if (ob != null)
                {
                    ob.SetActive(true);
                }
            }
            completionEvents.Invoke();
            complete = true;
        }
        return true;
    }

    /// <summary>
    /// Checks if this objective should be listening and changes its value
    /// </summary>
    public void CheckListening()
    {
        if (available)
        {
            foreach (Requirement requirement in requirements)
            {
                requirement.listening = true;
            }
        }
    }

    /// <summary>
    /// Updates this objective with a requirement event, updating all of its requirements
    /// </summary>
    /// <param name="e">Event to update with</param>
    public void UpdateObjective(RequirementEvent e)
    {
        CheckAvailable();
        foreach (Requirement r in requirements)
        {
            r.UpdateRequirement(e);
        }
        if (!complete)
        {
            foreach (ObjectiveObject obj in objectiveObjects)
            {
                obj.CheckObjectiveObject();
            }
        }
    }

    /// <summary>
    /// Force completes all requirements in this objective
    /// </summary>
    public void ForceCompleteObjective()
    {
        foreach(Requirement r in requirements)
        {
            r.ForceComplete();
        }
        UpdateObjective(new RequirementEvent(RequirementType.CompleteMinigame, PropFlags.None, true));
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CompleteMinigame, PropFlags.None, true));
    }

    // Override ToString to return formatted for To-Do list
    new public string ToString
    {
        get
        {
            if (CheckAvailable())
            {
                string value = "";
                value = objectiveName + "";
                bool allDone = true;
                foreach (Requirement r in requirements)
                {
                    if (r.listening)
                    {
                        value += "\n    - <size=-2>" + r.ToString + "</size>";
                    }
                    if (!r.CheckComplete())
                    {
                        allDone = false;
                    }
                }
                if (allDone)
                {
                    CheckComplete();
                    value = "<color=#111><s>" + objectiveName + "</s></color>";
                }
                return value;
            }
            return "";
        }
    }

    /// <summary>
    /// Sets the interal IDs of each requirement
    /// </summary>
    public void SerializeRequirements()
    {
        for(int i = 0; i < requirements.Count; i ++)
        {
            requirements[i].ID = i;
        }
    }

}