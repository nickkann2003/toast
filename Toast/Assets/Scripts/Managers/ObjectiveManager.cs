using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    // ------------------------------- Variables
    public static ObjectiveManager instance;

    [Header("Objective Groups")]
    [SerializeField]
    public List<ObjectiveGroup> groups = new List<ObjectiveGroup>();

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
}