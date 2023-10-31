using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;
    
    public List<Objective> objectives = new List<Objective>();
    public List<TextMeshPro> displays = new List<TextMeshPro>();

    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update Objectives
    public void UpdateObjectives(RequirementEvent e)
    {
        foreach (Objective obj in objectives)
        {
            obj.UpdateObjective(e);
        }
    }

    private void UpdateText()
    {
        
    }

    new public string ToString
    {
        get
        {
            string value = "";
            value += "To-Do List:\n";
            foreach(Objective obj in objectives)
            {
                value += "\n- " + obj.ToString();
            }
            return value;
        }
    }
}
