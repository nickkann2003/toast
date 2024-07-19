using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryHighlight : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    private Highlights myHighlight;
    private HighlightSettings previousSettings;

    [SerializeField] 
    private HighlightSettings tempHighlightSettings;

    [SerializeField, Button]
    private void TurnOnHighlight() { TurnOn(); }

    // ------------------------------- Functions -------------------------------

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
    }

    public void TurnOn()
    {
        StartCoroutine(SetTempHighlightOn());
    }

    public void TurnOff()
    {
        StartCoroutine(SetTempHighlightOff());
    }

    private IEnumerator SetTempHighlightOn()
    {
        yield return new WaitForFixedUpdate();
        previousSettings = myHighlight.defaultHighlightSetting;

        myHighlight.defaultHighlightSetting = tempHighlightSettings;

        myHighlight.SettingOutline();

        Outline objOutline = gameObject.GetComponent<Outline>();
        if (objOutline != null)
        {
            objOutline.enabled = true;
        }
    }

    private IEnumerator SetTempHighlightOff()
    {
        yield return new WaitForFixedUpdate();
        if (previousSettings != null)
        {
            myHighlight.defaultHighlightSetting = previousSettings;
            myHighlight.SettingOutline();
        }
    }

    private void GetReferences()
    {
        tempHighlightSettings = HighlightsReferences.instance.firstAppearanceHighlight;
        myHighlight = gameObject.GetComponent<Highlights>();
        if (myHighlight == null)
        {
            myHighlight = gameObject.AddComponent<Highlights>();
        }
    }
}
