using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Diagnostics;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective", order = 51)]
public class SO_Objective : ScriptableObject
{
    [Header("Objective Display Info")]
    [SerializeField]
    private string objectiveName; // The name of the objective
    [SerializeField]
    private string description; // The description of the objective

    [Header("Requirements")]
    [SerializeField]
    private List<Requirement> requirements = new List<Requirement>(); // List of all requirements for this objective

    [Header("Objective ID")]
    [SerializeField]
    private int id;

    public string ObjectiveName { get => objectiveName; set => objectiveName = value; }
    public string Description { get { return description; } }
    public List<Requirement> Requirements { get => requirements; set => requirements = value; }
    public int ID { get { return id; } }
}
