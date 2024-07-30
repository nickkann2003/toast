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

    [SerializeField]
    private StickyClicker stickyClicker;

    private bool stickysMade = false;

    // Start is called before the first frame update
    void Start()
    {
        notepadAngle = notepadStation.cameraRotation;
        itemContainer.SetActive(false);
        UpdateText(ObjectiveManager.instance.objectiveGroups);
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

        if (!stickysMade)
        {
            while (stickyHelpers.Count < objs.Count)
            {
                stickyHelpers.Add(null);
            }
            int objIndexer = 0;
            for (int i = 0; i < objs.Count; i++)
            {
                if (stickyHelpers[i] == null)
                {
                    // Create and place sticky helper
                    Vector3 topPos = notepad.transform.position;
                    Quaternion rot = notepad.transform.rotation;

                    GameObject stickyObject = Instantiate(stickyPrefab);
                    stickyObject.transform.parent = notepad.transform.parent;
                    stickyHelpers[i] = stickyObject.GetComponent<StickyHelper>();
                    stickyHelpers[i].notepadHelper = this;
                    stickyHelpers[i].displayText = stickyObject.GetComponentInChildren<TextMeshPro>();
                    stickyHelpers[i].stickyText = stickyNote;
                    stickyHelpers[i].group = objs[i];
                    stickyHelpers[i].clicker = stickyClicker;

                    stickyObject.transform.position = topPos;
                    stickyObject.transform.rotation = rot;

                    Vector3 localPos = Vector3.zero;
                    localPos.z = -0.55f;
                    localPos.x = -0.27f;
                    localPos.y = 0.4225f - (objIndexer * 0.09f);

                    stickyObject.transform.localPosition = localPos;

                    if (objs[i].displayOnNotepad)
                        objIndexer++;
                }
            }
            stickysMade = true;
        }

        for (int i = 0; i < objs.Count; i++)
        {
            notepadText = "";
            
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
                stickyHelpers[i].stickyDisplayText = obj.ToString;
                stickyHelpers[i].SetText(notepadText);
            }
            else
            {
                stickyHelpers[i].SetText("");
                stickyHelpers[i].transform.position = Vector3.zero;
            }
            stickyHelpers[i].SetStickyText(obj.ToString);
        }
        stickyHelpers[1].SelectText();
        notepad.text = "";
    }

    public void UpdateSticky(StickyHelper helper)
    {
        for(int i = 0; i < stickyHelpers.Count; i ++)
        {
            string notepadText = "";
            int objIndexer = 0;
            ObjectiveGroup obj = ObjectiveManager.instance.objectiveGroups[i];
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
                stickyHelpers[i].stickyDisplayText = obj.ToString;
                stickyHelpers[i].SetText(notepadText);
                objIndexer++;
            }
            else
            {
                stickyHelpers[i].SetText("");
                stickyHelpers[i].transform.position = Vector3.zero;
            }
            StickyHelper h = stickyHelpers[i];
            h.UnSelectText();
            h.SetText(notepadText);
            h.SetStickyText(obj.ToString);
        }
        helper.SelectText();
    }
}
