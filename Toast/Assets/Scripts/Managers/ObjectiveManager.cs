using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public static ObjectiveManager instance;

    [SerializeField]
    public List<ObjectiveGroup> objectiveGroups = new List<ObjectiveGroup>(); // DO NOT CHANGE VARIABLE NAME IT WILL WIPE ALL EDITOR INFO (im unbelievably sad)
    [NonSerialized]
    public Dictionary<int, Objective> objectivesById = new Dictionary<int, Objective>();

    [Header("Objectives by location"), Description("This is a collection of objectives and their necessary location, ordered from first incomplete to last")]
    [SerializeField]
    public List<ObjectiveDefaultStation> objectiveStations;
    public Station defaultLocation;

    private string objSerialPath = "Assets/Resources/objs.txt";

    [Header("Complete Objectives by ID")]
    [SerializeField]
    private int objectiveToComplete;
    [SerializeField, Button]
    private void ForceCompleteSpecificObjective()
    {
        ForceCompleteObjective(objectiveToComplete);
        objectiveToComplete++;
    }

    // Save Info
    private char objectiveMarker = '~';
    private char spacer = '_';
    private char requirementStartMarker = '[';
    private char requirementSpace = ',';

    [SerializeField]
    private NotepadHelper notepadHelper;

    // ------------------------------- Properties -------------------------------
    public Dictionary<int, Objective> ObjectivesById { get => objectivesById; set => objectivesById = value; }


    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;

        SortObjectivesById();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Run each groups OnLoad
        foreach(ObjectiveGroup group in objectiveGroups)
        {
            group.OnLoad();
        }

        // Run each groups LoadSaveData
        
        // Run each groups CheckAvailable
        
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDisable()
    {
        foreach(ObjectiveGroup group in objectiveGroups)
        {
            group.OnDisable();
        }
    }

    /// <summary>
    /// Updates the text of all objective groups
    /// </summary>
    public void UpdateText()
    {
        StartCoroutine(RunTextUpdate());
    }

    private IEnumerator RunTextUpdate()
    {
        yield return new WaitForFixedUpdate();
        foreach (ObjectiveGroup g in objectiveGroups)
        {
            g.CheckAllComplete();
        }
        foreach (ObjectiveGroup g in objectiveGroups)
        {
            g.CheckAvailable();
            g.UpdateText();
        }
        notepadHelper.UpdateText(objectiveGroups);
    }

    private void EditorTextUpdate()
    {
        foreach (ObjectiveGroup g in objectiveGroups)
        {
            g.CheckAllComplete();
        }
        foreach (ObjectiveGroup g in objectiveGroups)
        {
            g.CheckAvailable();
            g.UpdateText();
        }
    }

    /// <summary>
    /// Force completes an objective with a given ID
    /// </summary>
    public void ForceCompleteObjective(int id)
    {
        if (objectivesById.ContainsKey(id))
        {
            ObjectivesById[id].ForceCompleteObjective();
        }
        else
        {
            Debug.Log("Tried to force complete key " + id + ", which did not exist");
        }

        UpdateText();
    }

    /// <summary>
    /// Text display
    /// </summary>
    new public string ToString
    {
        get
        {
            string value = "";
            foreach (ObjectiveGroup g in objectiveGroups)
            {
                value += g.ToString();
            }
            return value;
        }
    }

    /// <summary>
    /// Returns a formatted string for saving Objective information
    /// </summary>
    /// <returns></returns>
    public string GetObjectiveStorageString()
    {
        SortObjectivesById();
        string formattedString = string.Empty;

        foreach(Objective obj in objectivesById.Values)
        {
            SO_Objective o = obj.ObjectiveInfo;
            formattedString += objectiveMarker;
            formattedString += o.ID;
            formattedString += spacer;
            formattedString += o.Complete ? "1" : "0";
            formattedString += spacer;
            formattedString += obj.CheckAvailable() ? "1" : "0";
            formattedString += requirementStartMarker;
            foreach(Requirement r in o.Requirements)
            {
                formattedString += r.ID;
                formattedString += spacer;
                formattedString += r.CheckComplete() ? "1" : "0";
                formattedString += spacer;
                formattedString += r.Current;
                formattedString += requirementSpace;
            }
            formattedString = formattedString.Substring(0, formattedString.Length - 1); // Trims off the last spacer
        }
        Debug.Log(formattedString);
        return formattedString;
    }

    /// <summary>
    /// Sorts objectives into a dictionary by ID
    /// </summary>
    private void SortObjectivesById()
    {
        foreach (ObjectiveGroup g in objectiveGroups)
        {
            foreach(Objective o in g.objectives)
            {
                objectivesById[o.ID] = o;
            }
        }
    }

    public void LoadObjectives(string fileDat)
    {
        // Sort objectives by ID into fictionary
        SortObjectivesById();

        foreach(Objective o in objectivesById.Values)
        {
            o.Complete = false;
        }

        // Get array of all objective objects from file data
        string[] allObjs = fileDat.Split(objectiveMarker);

        // For each objective array object
        foreach (string ob in allObjs)
        {
            // If its an empty string, ignore
            if (ob.Equals(""))
            {
                continue;
            }

            // Split obj info into obj and requirement info
            string[] tempObj = ob.Split(requirementStartMarker);
            string[] objDatSplit = tempObj[0].Split(spacer);
            int tId = int.Parse(objDatSplit[0]);
            bool tComplete = objDatSplit[1].Equals("1") ? true : false;
            bool tAvailable = objDatSplit[2].Equals("1") ? true : false;

            // If it was complete, force complete the objective
            if (tComplete)
            {
                ForceCompleteObjective(tId);
                continue;
            }

            // If incomplete, but available, update requirement data
            if (tAvailable)
            {
                if (objectivesById.ContainsKey(tId))
                {
                    string[] reqs = tempObj[1].Split(requirementSpace);
                    foreach (string req in reqs)
                    {
                        string[] reqsSplit = req.Split(spacer);
                        objectivesById[tId].SetRequirement(int.Parse(reqsSplit[0]), (reqsSplit[1].Equals("1") ? true : false), int.Parse(reqsSplit[2]));
                    }
                }
            }
        }

        SetPlayerStartingPosition();

        UpdateText();
    }

    /// <summary>
    /// Called when loading a save, checks the list of objectives and their locations in order, sending the player
    /// to the location of the first uncompleted objective, or the default if all are completed
    /// </summary>
    private void SetPlayerStartingPosition()
    {
        bool sent = false;
        foreach(ObjectiveDefaultStation s in objectiveStations)
        {
            if (!s.objective.Complete && !sent)
            {
                sent = true;
                StationManager.instance.MoveToStation(s.defaultStation, disableMoveBackwards: true);
            }

            if(!sent)
                StationManager.instance.MoveToStation(defaultLocation, disableMoveBackwards: false);
        }
    }

    /// <summary>
    /// Returns the total number of objectives in the game
    /// </summary>
    /// <returns>Total num objectives</returns>
    public int GetNumTotalObjectives()
    {
        int numObjs = 0;

        foreach(ObjectiveGroup g in objectiveGroups)
        {
            foreach(Objective o in g.objectives)
            {
                numObjs += 1;
            }
        }

        return numObjs;
    }

    private void OnDrawGizmosSelected()
    {
        instance = this;
        SortObjectivesById();
        EditorTextUpdate();
    }
}

[Serializable]
public class ObjectiveDefaultStation
{
    [SerializeField] public SO_Objective objective;
    [SerializeField] public Station defaultStation;
}