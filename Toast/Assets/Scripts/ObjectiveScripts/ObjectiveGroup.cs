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

    public bool complete = false;
    public bool available = false;

    [SerializeField]
    public bool displayOnNotepad = true;

    [SerializeField]
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
        bool av = false;
        foreach (Objective o in objectives)
        {
            if (o.CheckAvailable())
            {
                av = true;
            }
        }
        available = av;
    }

    /// <summary>
    /// Has each objective check if it is complete
    /// </summary>
    public void CheckAllComplete()
    {
        if (Application.isPlaying)
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

    public void SendToDisplayStation()
    {
        SendToDisplayStation(0);
    }

    public void SendToDisplayStation(int display)
    {
        if(displays.Count > display)
        {
            Station station = displays[display].GetComponentInParent<Station>();
            if(station != null)
            {
                StationManager.instance.MoveToStationThroughParents(station);
            }
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
            string title = "";
            if (complete)
            {
                title = "<color=#111><s>" + objectivesTitle + "</s></color>";
            }
            else if (available)
            {
                title = objectivesTitle;
            }
            else
            {
                title = "<color=#111>???</color>";
            }
            value += $"<u><b><pos=0%>{title}:</pos> <size=-5><color=#111><pos=85%>~</pos></size></color></b></u>";
            int completed = 0;
            int total = 0;
            //value += "\n" + "<color=#111><size=-1>" + "Objectives Completed: " + completedObjectives + "</size></color>";
            foreach (Objective obj in objectives)
            {
                total++;
                value += obj.ToString;

                if (obj.Complete)
                {
                    completed++;
                }
            }
            string[] subs = value.Split('~');
            value = subs[0].Substring(0, subs[0].Length) + completed + "/" + total + subs[1];
            return value;
        }
    }
}