using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public abstract class Station : MonoBehaviour, IHighlightable
{
    public Collider clickableCollider;

    private Outline outline;

    // If is dynamic, the station can be moved from place to place
    public bool isDynamic;

    // If isEnable is false, then the player should not be able to reach to interact with this station/prop
    public bool isEnabled;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled= true;
        this.outline = this.transform.AddComponent<Outline>();
    }

    public void EnableHiglight()
    {
        outline.enabled = true;
    }

    public void DisableHighlight()
    {
        outline.enabled = false;
    }

}
