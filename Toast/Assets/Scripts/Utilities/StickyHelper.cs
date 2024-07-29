using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StickyHelper : MonoBehaviour
{
    [SerializeField]
    public TextMeshPro displayText;
    public string text;
    [SerializeField]
    public TextMeshPro stickyText;
    [SerializeField]
    public NotepadHelper notepadHelper;

    public void SetStickyText(string newText)
    {
        stickyText.text = newText;
    }
    public void SetText(string text)
    {
        this.text = text;
    }

    public void SelectText()
    {
        displayText.text = "<size=+5>" + text + "</size>";
        SetStickyText(text);
    }

    public void UnSelectText()
    {
        displayText.text = text;
    }

    private void OnMouseDown()
    {
        notepadHelper.UpdateSticky(this);
    }

}
