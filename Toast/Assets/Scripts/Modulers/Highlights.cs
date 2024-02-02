using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Highlights : MonoBehaviour
{
    [SerializeField] private bool isHighlightedEnabled = true;
    [SerializeField] private List<Outline> outline = new List<Outline>();
    bool IsHighlightedEnable { get; set; }

    private void Start()
    {
        Outline defaultOutline;
        if(this.TryGetComponent<Outline>(out defaultOutline))
        {
            outline.Add(defaultOutline);
        }

        if(outline.Count == 0)
        {
            this.outline= new List<Outline>();
            Outline temp = this.AddComponent<Outline>();
            outline.Add(temp); 
        } 
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
