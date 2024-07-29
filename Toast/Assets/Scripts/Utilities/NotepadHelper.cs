using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotepadHelper : MonoBehaviour
{
    [SerializeField]
    private Station notepadStation;
    private Quaternion notepadAngle;
    [SerializeField]
    private GameObject itemContainer;
    bool atNotepad = false;

    [SerializeField]
    private TextMeshPro notepad;
    [SerializeField]
    private TextMeshPro stickyNote;

    // Start is called before the first frame update
    void Start()
    {
        notepadAngle = notepadStation.cameraRotation;
        itemContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
        if (!atNotepad)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void GoToNotepad()
    {
        //notepadStation.cameraRotation = Quaternion.identity;
        if (!atNotepad)
        {
            atNotepad = true;
            itemContainer.SetActive(true);
        }

    }

    public void LeaveNotepad()
    {
        //notepadStation.cameraRotation = notepadAngle;
        if (atNotepad)
        {
            StartCoroutine(AtNotepadFalse());
        }
    }

    public void ToggleStation()
    {
        if(StationManager.instance.playerLocation == notepadStation)
        {
            StationManager.instance.StationMoveBack();
        }
        else
        {
            StationManager.instance.MoveToStation(notepadStation);
        }
    }

    private IEnumerator AtNotepadFalse()
    {
        yield return new WaitForSeconds(0.4f);
        atNotepad = false;
        itemContainer.SetActive(false);
    }

    public void UpdateText(List<ObjectiveGroup> objs)
    {
        // Notepad ------------------
        string notepadText = "";

        //foreach(Objective obj in objs)
        //{
        //    if(obj.)
        //    if (obj.objectiveInfo.Available)
        //    {
        //        notepadText += obj.objectiveInfo.ObjectiveName;
        //        notepadText += "\n";
        //    }
        //    else if(obj.objectiveInfo.UnavailableText != string.Empty)
        //    {
        //        notepadText += "<color=#111><size=-10>" + obj.objectiveInfo.UnavailableText + "</size></color>";
        //        notepadText += "\n";
        //    }
        //}

        foreach (ObjectiveGroup obj in objs)
        {
            if (obj.displayOnNotepad)
            {
                if (obj.complete)
                {
                    notepadText += "<color=#111><s>" + obj.objectivesTitle + "</s></color>";
                    notepadText += "\n";
                }
                else if (obj.available)
                {
                    notepadText += obj.objectivesTitle;
                    notepadText += "\n";
                }
                else
                {
                    notepadText += "<color=#111>???</color>";
                    notepadText += "\n";
                }
            }
        }

        notepad.text = notepadText;

        // Sticky -------------------
        string stickyText = "";

        stickyNote.text = stickyText;
    }
}
