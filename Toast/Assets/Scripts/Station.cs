using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Build.Content;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHighlightable
{
    public Collider clickableCollider;

    private Outline outline;

    // If is dynamic, the station can be moved from place to place
    public bool isDynamic;

    // If isEnable is false, then the player should not be able to reach to interact with this station/prop
    [SerializeField] private bool isEnabled;

    bool IHighlightable.IsHighlightedEnable => isEnabled;

    Outline IHighlightable.Outline => outline;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled= true;
        //this.transform.AddComponent<Outline>();
        if(!TryGetComponent<Outline>(out outline))
        {
            this.transform.AddComponent<Outline>();
            outline = GetComponent<Outline>();
        }
        outline.enabled = false;
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
        if (outline.isActiveAndEnabled)
            outline.enabled = false;
    }

}
