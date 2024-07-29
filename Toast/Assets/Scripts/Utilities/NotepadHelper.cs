using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private List<StickyHelper> stickyHelpers = new List<StickyHelper>();

    [SerializeField]
    private GameObject stickyPrefab;

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

        while (stickyHelpers.Count < objs.Count)
        {
            stickyHelpers.Add(null);
        }

        for (int i = 0; i < objs.Count; i++)
        {
            ObjectiveGroup obj = objs[i];
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

            if (stickyHelpers[i] == null)
            {
                // Create and place sticky helper
                Vector3 topPos = notepad.transform.position;
                Quaternion rot = notepad.transform.rotation;

                GameObject stickyObject = Instantiate(stickyPrefab);
                stickyObject.transform.parent = itemContainer.transform;
                stickyHelpers[i] = stickyObject.GetComponent<StickyHelper>();
                stickyHelpers[i].displayText = stickyNote;
                stickyObject.transform.position = topPos;
                stickyObject.transform.rotation = rot;
            }
            stickyHelpers[i].SetText(obj.ToString());
        }
    }

    public void UpdateSticky(StickyHelper helper)
    {
        for(int i = 0; i < stickyHelpers.Count; i ++)
        {
            StickyHelper h = stickyHelpers[i];
            h.UnSelectText();
            h.SetText(ObjectiveManager.instance.objectiveGroups[i].ToString());
        }
        helper.SelectText();
    }
}
