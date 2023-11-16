using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Build.Content;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHighlightable
{
    public Collider clickableCollider;
    public GameObject stationHighlight;
    private Outline outline;

    // If isEnable is false, then the player should not be able to reach to interact with this station/prop
    [SerializeField] private bool isEnabled;

    bool IHighlightable.IsHighlightedEnable => isEnabled;

    Outline IHighlightable.Outline => outline;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled= true;
        
        if(!TryGetComponent<Outline>(out outline))
        {
            this.transform.AddComponent<Outline>();
            outline = GetComponent<Outline>();
        }
        outline.enabled = false;
        if(clickableCollider!= null)
        {
            clickableCollider = GetComponent<Collider>();
        }
    }

    public void TurnOnHighlght()
    {
        if(!outline.isActiveAndEnabled)
        {
            outline.enabled = true;
        }
    }

    public void TurnOffHighlight()
    {
        if(outline != null)
        {
        if (outline.isActiveAndEnabled)
            outline.enabled = false;
        }
    }

}
