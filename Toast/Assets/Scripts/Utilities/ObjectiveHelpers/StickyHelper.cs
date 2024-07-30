using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickyHelper : MonoBehaviour
{
    [SerializeField]
    public TextMeshPro displayText;
    public string text;
    public string stickyDisplayText;
    [SerializeField]
    public TextMeshPro stickyText;
    [SerializeField]
    public NotepadHelper notepadHelper;
    public StickyClicker clicker;

    public ObjectiveGroup group;

    public void SetStickyText(string newText)
    {
        stickyText.text = newText;
    }
    public void SetText(string text)
    {
        this.text = text;
        displayText.text = text;
    }

    public void SelectText()
    {
        clicker.currentHelper = this;
        displayText.text = "<size=+5>" + text + "</size>";
        stickyText.text = stickyDisplayText;
    }

    public void UnSelectText()
    {
        displayText.text = text;
    }

    private void OnMouseDown()
    {
        notepadHelper.UpdateSticky(this);
    }

    public void SendToObjectivePaper()
    {
        group.SendToDisplayStation();
    }

}
