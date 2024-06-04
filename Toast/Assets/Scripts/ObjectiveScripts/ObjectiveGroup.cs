using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ObjectiveGroup
{
    // ------------------------------- Variables -------------------------------
    [Header("Display Name")]
    public string name;

    [Header("Objectives")]
    public List<Objective> objectives;

    [Header("Displays")]
    public List<TextMeshPro> displays;
    public List<TextMeshProUGUI> displaysUI;

    private float completedObjectives = 0;
    private float oldCompletedObjectives = 0; // Used to check if some new objective completed

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

    // Update Objectives
    public void UpdateObjectives(RequirementEvent e)
    {
        completedObjectives = 0;
        for(int i = objectives.Count - 1; i >= 0; i --)
        {
            Objective obj = objectives[i];
            //obj.UpdateObjective(e);
            if (obj.CheckComplete())
            {
                completedObjectives++;
            }
        }
        for (int i = objectives.Count - 1; i >= 0; i--)
        {
            Objective obj = objectives[i];
            obj.CheckAvailable();
            //obj.CheckListening();
        }
        if (completedObjectives > oldCompletedObjectives)
        {
            AudioManager.instance.PlayOneShotSound(AudioManager.instance.objectiveComplete, 0.3f, 1);
            Debug.LogWarning("new objective complete");
        }
        oldCompletedObjectives = completedObjectives;
        UpdateText();
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
                    value += "\n- " + "<size=-1>" + obj.ToString + "</size>";
                }
            }
            return value;
        }
    }
}