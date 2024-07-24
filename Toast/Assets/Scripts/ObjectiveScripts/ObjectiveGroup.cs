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
    public string objectivesTitle;
    public List<Objective> objectives;
    public List<TextMeshPro> displays;
    public List<TextMeshProUGUI> displaysUI;

    private bool complete = false;

    private bool destroyPaperOnCompletion = true;

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

    public void OnDisable()
    {
        foreach (Objective o in objectives)
        {
            o.OnDisable();
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
        bool allComplete = true;
        foreach(Objective o in objectives)
        {
            if (!o.CheckComplete())
            {
                allComplete = false;
            }
        }
        if (allComplete && !complete)
        {
            complete = true;
            if (destroyPaperOnCompletion)
            {
                foreach(TextMeshPro display in displays)
                {
                    GameObject.Destroy(display.transform.parent.gameObject);
                }
                displays.Clear();
            }
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
            value += $"<u><b><pos=0%>{objectivesTitle}:</pos> <size=-5><color=#111><pos=85%>~</pos></size></color></b></u>";
            int complete = 0;
            int total = 0;
            //value += "\n" + "<color=#111><size=-1>" + "Objectives Completed: " + completedObjectives + "</size></color>";
            foreach (Objective obj in objectives)
            {
                total++;
                value += obj.ToString;

                if (obj.Complete)
                {
                    complete++;
                }
            }
            string[] subs = value.Split('~');
            value = subs[0].Substring(0, subs[0].Length) + complete + "/" + total + subs[1];
            return value;
        }
    }
}