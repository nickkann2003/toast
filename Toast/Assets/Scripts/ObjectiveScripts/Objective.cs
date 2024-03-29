using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;

public class Objective : MonoBehaviour
{
    // Public variables
    public string objectiveName;
    public List<Objective> prerequisites = new List<Objective>();
    public List<Requirement> requirements = new List<Requirement>();
    public List<ObjectiveObject> objectiveObjects = new List<ObjectiveObject>();
    public List<GameObject> activatables = new List<GameObject>();
    public UnityEvent completionEvents;

    // Private variables
    private bool complete = false;

    // Properties
    public bool Complete { get => complete; }


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
                    return false;
                }
            }
        }
        foreach (Requirement requirement in requirements)
        {
            requirement.listening = true;
        }
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
}