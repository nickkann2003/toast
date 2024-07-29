using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadHelper : MonoBehaviour
{
    [SerializeField]
    private Station notepadStation;
    private Quaternion notepadAngle;
    [SerializeField]
    private GameObject itemContainer;
    bool atNotepad = false;

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
}
