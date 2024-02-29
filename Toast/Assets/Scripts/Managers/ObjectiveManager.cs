using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [SerializeField]
    public List<ObjectiveGroup> groups = new List<ObjectiveGroup>();

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
        foreach(ObjectiveGroup g in groups)
        {
            g.UpdateObjectives(e);
        }
    }

    private void UpdateText()
    {
        foreach (ObjectiveGroup g in groups)
        {
            g.UpdateText();
        }
    }

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
}