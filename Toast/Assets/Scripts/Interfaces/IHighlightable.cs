using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for highlightable objects
/// It should set up cursor, outline of the objects
/// </summary>
public interface IHighlightable
{
    bool IsHighlightedEnable { get; }
    Outline Outline { get; }
    void TurnOnHighlght();
    void TurnOffHighlight();
}
