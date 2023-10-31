using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Objective : MonoBehaviour
{
    // Public variables
    public List<Objective> prerequisites = new List<Objective>();
    public List<Requirement> requirements = new List<Requirement>();
    public string objectiveName;

    // Private variables
    private bool complete = false;

    // Properties
    public bool Complete { get => complete; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Check if the current task has had its prerequisites complete
    public bool CheckAvailable()
    {
        foreach(Objective obj in prerequisites)
        {
            if (!obj.Complete)
            {
                return false;
            }
        }
        return true;
    }

    // Check if this objective is complete
    public bool CheckComplete()
    {
        foreach(Requirement r in requirements)
        {
            if (!r.CheckComplete())
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateObjective(RequirementEvent e)
    {
        foreach(Requirement r in requirements)
        {
            r.UpdateRequirement(e);
        }
    }

    // Override ToString to return formatted for To-Do list
    new public string ToString
    {
        get
        {
            string value = "";
            value = objectiveName + ":";
            bool allDone = true;
            foreach(Requirement r in requirements )
            {
                if(r.listening)
                {
                value += "\n    - " + r.ToString;
                if (!r.CheckComplete())
                {
                    allDone = false;
                }
                }
            }
            if(allDone)
            {
                value = "COMPLETE: " + value;
            }
            return value;
        }
    }
}
