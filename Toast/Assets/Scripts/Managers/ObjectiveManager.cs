using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    
    public List<Objective> objectives = new List<Objective>();
    public List<TextMeshPro> displays = new List<TextMeshPro>();

    private float completedObjectives = 0;

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

    // Update Objectives
    public void UpdateObjectives(RequirementEvent e)
    {
        completedObjectives = 0;
        foreach (Objective obj in objectives)
        {
            obj.UpdateObjective(e);
            if (obj.CheckComplete())
            {
                completedObjectives++;
            }
        }
        UpdateText();
    }

    private void UpdateText()
    {
        foreach(TextMeshPro display in displays)
        {
            display.text = this.ToString;
        }
    }

    new public string ToString
    {
        get
        {
            string value = "";
            value += "<u>To-Do List:</u>";            
            value += "\n" + "<color=#111><size=-1>" + "Objectives Completed: " + completedObjectives + "</size></color>";
            foreach (Objective obj in objectives)
            {
                if (obj.CheckAvailable() && !obj.CheckComplete())
                {
                    value += "\n- " + "<size=-1>" + obj.ToString + "</size>";
                }
            }
            return value;
        }
    }
}
