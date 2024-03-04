using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ObjectiveGroup
{
    public List<Objective> objectives;
    public List<TextMeshPro> displays;
    public List<TextMeshProUGUI> displaysUI;

    private float completedObjectives = 0;
    private float oldCompletedObjectives = 0; // Used to check if some new objective completed

    private float completedRequirements = 0;
    private float oldCompletedRequirements = 0;

    // Update Objectives
    public void UpdateObjectives(RequirementEvent e)
    {
        completedObjectives = 0;
        completedRequirements = 0;
        foreach (Objective obj in objectives)
        {
            obj.UpdateObjective(e);
            foreach(Requirement r in obj.requirements)
            {
                if (r.CheckComplete())
                {
                    completedRequirements++;
                }
            }
            if (obj.CheckComplete())
            {
                completedObjectives++;
            }
        }
        if(completedRequirements > oldCompletedRequirements)
        {
            AudioManager.instance.PlayOneShotSound(AudioManager.instance.requirementComplete, 0.8f, 1);
            Debug.LogWarning("new requirement complete");
        }
        if (completedObjectives > oldCompletedObjectives)
        {
            AudioManager.instance.PlayOneShotSound(AudioManager.instance.objectiveComplete, 0.3f, 1);
            Debug.LogWarning("new objective complete");
            GameManager.Instance.TutorialToGame();
        }
        oldCompletedObjectives = completedObjectives;
        oldCompletedRequirements = completedRequirements;
        UpdateText();
    }

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