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
    private bool complete = false;

    [Header("Pre-Requisite Objectives")]
    public List<int> prerequisiteIds = new List<int>();

    [Header("Activatables Upon Completion")]
    public List<GameObject> activatables = new List<GameObject>();

    [Header("Unity Events")]
    public UnityEvent completionEvents;

    // ------------------------------- Properties -------------------------------
    public bool Complete { get => objectiveInfo.Complete; }
    public int ID { get => objectiveInfo.ID; }
    public SO_Objective ObjectiveInfo { get => objectiveInfo; set => objectiveInfo = value; }

    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Functions to be run once this objective loads
    /// </summary>
    public void OnLoad()
    {
        objectiveInfo.OnLoad();
        foreach(var obj in activatables)
        {
            obj.SetActive(false);
        }
    }

    // Check if the current task has had its prerequisites complete
    public bool CheckAvailable()
    {
        if (prerequisiteIds.Count > 0)
        {
            foreach (int i in prerequisiteIds)
            {
                Objective obj = ObjectiveManager.instance.ObjectivesById[i];
                if (!obj.Complete)
                {
                    return false;
                }
            }
        }
        objectiveInfo.SetAvailable();
        return true;
    }

    // Check if this objective is complete
    public bool CheckComplete()
    {
        if (!complete)
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
                // One Shot Effects
                complete = true;
                completionEvents.Invoke();
                AudioManager.instance.PlayOneShotSound(AudioManager.instance.objectiveComplete);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Force completes all requirements in this objective
    /// </summary>
    public void ForceCompleteObjective()
    {
        if (!complete)
        {
            objectiveInfo.ForceComplete();
            foreach (GameObject ob in activatables)
            {
                if (ob != null)
                {
                    ob.SetActive(true);
                }
            }
            complete = true;
            completionEvents.Invoke();
        }
    }

    // Override ToString to return formatted for To-Do list
    new public string ToString
    {
        get
        {
            return objectiveInfo.ToString;
        }
    }
}