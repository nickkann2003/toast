using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public static ObjectiveManager instance;

    [Header("Objective Groups")]
    [SerializeField]
    public List<ObjectiveGroup> groups = new List<ObjectiveGroup>();

    private string objSerialPath = "Assets/Resources/objs.txt";

    [SerializeField, Button]
    private void SerializeAllObjectives() { SerializeObjectives(); }
    [SerializeField, Button]
    private void DONOTPRESSResetObjectiveSerialization() 
    { 
        foreach(ObjectiveGroup g in groups)
        {
            foreach(Objective o in g.objectives)
            {
                o.ID = -1;
            }
        }
        // Write out reset cId
        StreamWriter wr = new StreamWriter(objSerialPath);
        wr.Write(0);
        wr.Close();
    }

    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates all objectives with a given requirement event
    /// </summary>
    /// <param name="e">Event</param>
    public void UpdateObjectives(RequirementEvent e)
    {
        foreach(ObjectiveGroup g in groups)
        {
            g.UpdateObjectives(e);
        }
    }

    /// <summary>
    /// Updates the text of all objective groups
    /// </summary>
    private void UpdateText()
    {
        foreach (ObjectiveGroup g in groups)
        {
            g.UpdateText();
        }
    }

    /// <summary>
    /// Text display
    /// </summary>
    new public string ToString
    {
        get
        {
            string value = "";
            foreach (ObjectiveGroup g in groups)
            {
                value += g.ToString();
            }
            return value;
        }
    }
    /// <summary>
    /// Sets the interal IDs of each objective
    /// </summary>
    private void SerializeObjectives()
    {
        // Read in the file and get the current ID of objectives
        
        StreamReader sr = new StreamReader(objSerialPath);
        string firstLine = sr.ReadLine();
        int cId = int.Parse(firstLine);
        sr.Close();

        foreach(ObjectiveGroup g in groups)
        {
            foreach (Objective o in g.objectives)
            {
                if (o.ID == -1)
                {
                    o.ID = cId;
                    cId += 1;
                }
                o.SerializeRequirements();
            }
        }

        // Write out the new cID of objectives
        StreamWriter wr = new StreamWriter(objSerialPath);
        wr.Write(cId);
        wr.Close();

        Debug.Log("Serialize Success! Next ID: " + cId);
    }
}