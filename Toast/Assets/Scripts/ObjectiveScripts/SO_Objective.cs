using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Diagnostics;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Objective", menuName = "Objective", order = 51)]
public class SO_Objective : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [Header("Display Info")]
    [SerializeField]
    private string objectiveName; // The name of the objective
    [SerializeField]
    private string description; // The description of the objective
    [SerializeField]
    private string completeText = ""; // Text displayed once this objective is complete
    [SerializeField]
    private string unavailableText = "???";


    [Header("Requirements")]
    [SerializeField]
    private List<Requirement> requirements = new List<Requirement>(); // List of all requirements for this objective
    [SerializeField]
    private bool sequentialRequirements = true;

    [Header("ID")]
    [SerializeField]
    private int id;

    [Header("Progress Info")]
    [SerializeField]
    private bool complete;
    [SerializeField]
    private bool available;
    [SerializeField]
    private bool successorComplete;

    // ------------------------------- Properties -------------------------------
    public string ObjectiveName { get => objectiveName; set => objectiveName = value; }
    public bool Complete { get => complete; set => complete = value; }
    public bool Available { get => available; set => available = value; }
    public string Description { get { return description; } }
    public List<Requirement> Requirements { get => requirements; set => requirements = value; }
    public int ID { get { return id; } }

    // ------------------------------- Functions -------------------------------
    public void OnLoad()
    {
        complete = false;
        available = false;
        successorComplete = false;
        foreach (Requirement r in requirements)
        {
            r.OnLoad();
        }
    }

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

    /// <summary>
    /// Sets available to true and sets listening values of all requirements
    /// </summary>
    public void SetAvailable()
    {
        available = true;
        CheckListening();
    }

    /// <summary>
    /// Checks if this objective should be listening and changes its value
    /// </summary>
    public void CheckListening()
    {
        if (available)
        {
            if (!sequentialRequirements)
            {
                foreach (Requirement requirement in requirements)
                {
                    requirement.listening = true;
                }
            }
            else
            {
                foreach(Requirement requirement in requirements)
                {
                    if (requirement.Complete)
                    {
                        continue;
                    }
                    requirement.listening = true;
                    break;
                }
            }
        }
    }

    public Requirement GetRequirement(int id)
    {
        foreach(Requirement r in requirements)
        {
            if(r.id == id)
            {
                return r;
            }
        }
        return null;
    }

    public void CompleteSuccessor()
    {
        successorComplete = true;
    }

    /// <summary>
    /// Force completes this objective and all its requirements
    /// </summary>
    public void ForceComplete()
    {
        foreach (Requirement r in requirements)
        {
            r.ForceComplete();
        }
        CheckAllRequirementsComplete();
    }

    public void OnDisable()
    {
        foreach(Requirement r in requirements)
        {
            r.OnDisable();
        }
    }

    new public string ToString
    {
        get
        {
            if (available)
            {
                string value = "";
                bool firstIncompleteRequirement = true;
                int complete = 0;
                int total = 0;
                //value = objectiveName + " <size=-5><color=#111>~ \n<i>" + description + "</i></color></size>";
                value = "<align=center>";
                foreach (Requirement r in requirements)
                {
                    if (r.listening)
                    {
                        if (firstIncompleteRequirement && !r.Complete)
                        {
                            value += "<size=-2>\n" + r.ToString + "</size>";
                        }
                        else 
                        {
                            value += "<size=-6>\n" + r.ToString + "</size>";
                        }
                    }
                    if (!r.Complete)
                    {
                        firstIncompleteRequirement = false;
                    }
                    else
                    {
                        complete++;
                    }
                    total++;
                }
                //string[] subs = value.Split('~');
                //value = subs[0].Substring(0, subs[0].Length) + complete + "/" + total + subs[1];
                if (CheckAllRequirementsComplete())
                {
                    //value = "\n<color=#111>  <s>" + objectiveName + "</s></color>";
                    value = "<align=center><color=#111>";
                    if (completeText != "" && !successorComplete)
                    {
                        value += "\n<color=#080808><size=-6>" + completeText + "</size></color>";
                    }
                }


                return value;
            }
            else if(unavailableText != "")
            {
                return "\n<align=center><size=-4><color=#111>" + unavailableText + "</color></size>";
            }
            return "";
        }
    }
}
