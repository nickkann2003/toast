// Latest Update: 2/1/2024
// Runi Jiang

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Highlights is a moduler/component that can be attached to anything that you want it to have outline
/// </summary>
public class Highlights : MonoBehaviour
{
    [SerializeField] private bool isHighlightedEnabled = true;  // Default is true
    // - This is used for some highlight parts that are made of several models
    // then you want to manually drag the Outline in the scene to each parts and then
    // drag those outlines to this list
    // - If you don't have a special outline, then you don't need to drag anything in the editor
    [SerializeField] private List<Outline> outline = new List<Outline>(); 
    bool IsHighlightedEnable { get; set; }

    private void Start()
    {
        Outline defaultOutline;
        // Check if there is a defualtOutline attached to the object already
        if(this.TryGetComponent<Outline>(out defaultOutline))
        {
            outline.Add(defaultOutline);
        }

        // If no, add an outline to the object
        if(outline.Count == 0)
        {
            this.outline= new List<Outline>();
            Outline temp = this.AddComponent<Outline>();
            outline.Add(temp); 
        } 

        // Disable all the outlines
        foreach(Outline outline in outline)
        {
            outline.enabled= false;
        }
    }

    public void TurnOnHighlght()
    {
        if(isHighlightedEnabled)
        {
            foreach(Outline outline in outline) {

                outline.enabled = true;

            }
        }
    }

    public void TurnOffHighlight()
    {
        if (isHighlightedEnabled)
        {
            foreach (Outline outline in outline)
            {
                outline.enabled = false;
            }
        }
    }
}
