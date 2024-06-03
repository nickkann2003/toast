using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;


//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
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
    public SO_Objective ObjectiveInfo { get => objectiveInfo; set => objectiveInfo = value; }

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
            if (objectiveInfo.CheckAllRequirementsComplete())
            {
                foreach (GameObject ob in activatables)
                {
                    if (ob != null)
                    {
                        ob.SetActive(true);
                    }
                }
                completionEvents.Invoke();
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    ///// <summary>
    ///// Updates this objective with a requirement event, updating all of its requirements
    ///// </summary>
    ///// <param name="e">Event to update with</param>
    //public void UpdateObjective(RequirementEvent e)
    //{
    //    CheckAvailable();
    //    foreach (Requirement r in requirements)
    //    {
    //        r.UpdateRequirement(e);
    //    }
    //    if (!complete)
    //    {
    //        foreach (ObjectiveObject obj in objectiveObjects)
    //        {
    //            obj.CheckObjectiveObject();
    //        }
    //    }
    //}

    /// <summary>
    /// Force completes all requirements in this objective
    /// </summary>
    //public void ForceCompleteObjective()
    //{
    //    foreach(Requirement r in requirements)
    //    {
    //        r.ForceComplete();
    //    }
    //    UpdateObjective(new RequirementEvent(RequirementType.CompleteMinigame, PropFlags.None, true));
    //    ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CompleteMinigame, PropFlags.None, true));
    //}

    // Override ToString to return formatted for To-Do list
    new public string ToString
    {
        get
        {
            return objectiveInfo.ToString();
        }
    }


    ///// <summary>
    ///// Sets the interal IDs of each requirement
    ///// </summary>
    //public void SerializeRequirements()
    //{
    //    for(int i = 0; i < requirements.Count; i ++)
    //    {
    //        requirements[i].ID = i;
    //    }
    //}

}