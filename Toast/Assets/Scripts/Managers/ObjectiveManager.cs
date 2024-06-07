using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public static ObjectiveManager instance;

    [SerializeField]
    public List<ObjectiveGroup> objectiveGroups = new List<ObjectiveGroup>(); // DO NOT CHANGE VARIABLE NAME IT WILL WIPE ALL EDITOR INFO (im unbelievably sad)
    private Dictionary<int, Objective> objectivesById = new Dictionary<int, Objective>();

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

    // ------------------------------- Buttons -------------------------------

    /// <summary>
    /// WARNING: This will reset all objective IDs, this can cause potential issues with
    /// saved file data. Make sure you know what you are doing before pressing this
    /// </summary>
    //public bool ResetObjectives = false;
    //[SerializeField, Button, EnableIf("ResetObjectives")]
    //private void DONOTPRESSResetObjectiveSerialization() 
    //{ 
    //    foreach(ObjectiveGroup g in groups)
    //    {
    //        foreach(Objective o in g.objectives)
    //        {
    //            o.ID = -1;
    //        }
    //    }
    //    // Write out reset cId
    //    StreamWriter wr = new StreamWriter(objSerialPath);
    //    wr.Write(0);
    //    wr.Close();
    //}

    /// <summary>
    /// Button for Serializing all Objectives, gives an ID to all OBJS without one
    /// </summary>
    //[SerializeField, Button]
    //private void SerializeAllObjectives() { SerializeObjectives(); }

    //[SerializeField, Button]
    //private void CompileStorageString() { GetObjectiveStorageString(); }
    //
    //[SerializeField, Button]
    //private void SaveObjectiveData() { SaveHandler.instance.SaveObjectiveData(GetObjectiveStorageString()); }

    // ------------------------------- Properties -------------------------------
    public Dictionary<int, Objective> ObjectivesById { get => objectivesById; set => objectivesById = value; }


    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SortObjectivesById();

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
    }

    /// <summary>
    /// Force completes an objective with a given ID
    /// </summary>
    public void ForceCompleteObjective(int id)
    {
        ObjectivesById[id].ForceCompleteObjective();
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
    /// Sets the interal IDs of each objective
    /// </summary>
    //private void SerializeObjectives()
    //{
    //    // Read in the file and get the current ID of objectives
    //    
    //    StreamReader sr = new StreamReader(objSerialPath);
    //    string firstLine = sr.ReadLine();
    //    int cId = int.Parse(firstLine);
    //    sr.Close();
    //
    //    List<int> foundIds = new List<int>();
    //
    //    foreach(ObjectiveGroup g in groups)
    //    {
    //        foreach (Objective o in g.objectives)
    //        {
    //            if (o.ID == -1 || foundIds.Contains(o.ID))
    //            {
    //                o.ID = cId;
    //                cId += 1;
    //            }
    //            o.SerializeRequirements();
    //            foundIds.Add(o.ID);
    //        }
    //    }
    //
    //    // Write out the new cID of objectives
    //    StreamWriter wr = new StreamWriter(objSerialPath);
    //    wr.Write(cId);
    //    wr.Close();
    //
    //    Debug.Log("Serialize Success! Next ID: " + cId);
    //}

    /// <summary>
    /// Returns a formatted string for saving Objective information
    /// </summary>
    /// <returns></returns>
    public string GetObjectiveStorageString()
    {
        SortObjectivesById();
        string formattedString = string.Empty;

        char objectiveMarker = '~';
        char spacer = '_';
        char requirementStartMarker = '[';
        char requirementSpace = ',';

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
}