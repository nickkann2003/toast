using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;


//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    // Public variables
    [Header("Objective Information")]
    public string objectiveName;

    [Header("Pre-Requisite Objectives")]
    public List<Objective> prerequisites = new List<Objective>();

    [Header("Requirements")]
    public List<Requirement> requirements = new List<Requirement>();

    [Header("Activtables Upon Completion")]
    public List<GameObject> activatables = new List<GameObject>();
    
    [Header("Objective Related Objects")]
    public List<ObjectiveObject> objectiveObjects = new List<ObjectiveObject>();

    [Header("Unity Events")]
    public UnityEvent completionEvents;

    // Private variables
    private bool complete = false;
    private bool available = false;

    // ID TRACKER DO NOT CHANGE
    public int id = -1;

    // ------------------------------- Properties -------------------------------
    public bool Complete { get => complete; }
    public int ID { get => id; set => id = value; }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        if (!complete)
        {
            foreach (GameObject obj in activatables)
            {
                obj.SetActive(false);
            }
        }

        if (!CheckAvailable())
        {
            foreach (Requirement req in requirements)
            {
                req.listening = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Check if the current task has had its prerequisites complete
    public bool CheckAvailable()
    {
        if (prerequisites.Count > 0)
        {
            foreach (Objective obj in prerequisites)
            {
                if (!obj.Complete)
                {
                    available = false;
                    return false;
                }
            }
        }
        available = true;
        return true;
    }

    // Check if this objective is complete
    public bool CheckComplete()
    {
        if (!complete)
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