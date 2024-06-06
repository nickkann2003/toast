using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ObjectiveGroup
{
    // ------------------------------- Variables -------------------------------
    public string name;
    public List<Objective> objectives;
    public List<TextMeshPro> displays;
    public List<TextMeshProUGUI> displaysUI;

    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Runs when this objective group is loaded
    /// </summary>
    public void OnLoad()
    {
        foreach(Objective o in objectives)
        {
            o.OnLoad();
        }
    }

    /// <summary>
    /// Has each objective check if it is available
    /// </summary>
    public void CheckAvailable()
    {
        foreach (Objective o in objectives)
        {
            o.CheckAvailable();
        }
    }

    /// <summary>
    /// Has each objective check if it is complete
    /// </summary>
    public void CheckAllComplete()
    {
        foreach(Objective o in objectives)
        {
            o.CheckComplete();
        }
    }

    /// <summary>
    /// Updates the display texts with objective info
    /// </summary>
    public void UpdateText()
    {
        foreach (TextMeshPro display in displays)
        {
            display.text = this.ToString;
        }

        foreach (TextMeshProUGUI display in displaysUI)
        {
            display.text = this.ToString;
        }
    }

    /// <summary>
    /// To String, formatted for user display
    /// </summary>
    new public string ToString
    {
        get
        {
            string value = "";
            value += "<u>To-Do List:</u>";
            //value += "\n" + "<color=#111><size=-1>" + "Objectives Completed: " + completedObjectives + "</size></color>";
            foreach (Objective obj in objectives)
            {
                if (obj.CheckAvailable())
                {
                    value += "\n" + "<size=-1>" + obj.ToString + "</size>";
                }
                else
                {
                    value += "\n" + "<size=-2>" + obj.ToString + "</size>";
                }
            }
            return value;
        }
    }
}