using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Diagnostics;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective", order = 51)]
public class SO_Objective : ScriptableObject
{
    [Header("Display Info")]
    [SerializeField]
    private string objectiveName; // The name of the objective
    [SerializeField]
    private string description; // The description of the objective

    [Header("Progress Info")]
    [SerializeField]
    private bool complete;
    [SerializeField]
    private bool available;

    [Header("Requirements")]
    [SerializeField]
    private List<Requirement> requirements = new List<Requirement>(); // List of all requirements for this objective

    [Header("ID")]
    [SerializeField]
    private int id;

    public string ObjectiveName { get => objectiveName; set => objectiveName = value; }
    public bool Complete { get => complete; set => complete = value; }
    public bool Available { get => available; set => available = value; }
    public string Description { get { return description; } }
    public List<Requirement> Requirements { get => requirements; set => requirements = value; }
    public int ID { get { return id; } }

    /// <summary>
    /// Checks all the requirements and returns true if all are complete
    /// </summary>
    /// <returns></returns>
    public bool CheckAllRequirementsComplete()
    {
        bool allComplete = true;
        foreach (Requirement r in requirements)
        {
            if (!r.CheckComplete())
            {
                allComplete = false;
            }
        }

        if (allComplete)
        {
            complete = true;
        }

        return allComplete;
    }
}
