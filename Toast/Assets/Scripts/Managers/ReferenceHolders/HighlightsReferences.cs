using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightsReferences : MonoBehaviour
{
    public static HighlightsReferences instance;

    public HighlightSettings firstAppearanceHighlight;

    private void Awake()
    {
        instance = this;
    }
}
