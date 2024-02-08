using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Highlight Settings", menuName = "Highlight Settings")]
public class HighlightSettings : ScriptableObject
{
    public ObjectsType objectsType = ObjectsType.Station;
    //public bool isHighlightedEnabled = true;
    public Color highlightColor = Color.yellow;
    public float highlightWidth;
    // Add other settings as needed
}
