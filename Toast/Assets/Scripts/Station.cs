using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Build.Content;
using UnityEngine;

[RequireComponent(typeof(Highlights))]
public abstract class Station : MonoBehaviour
{
    public Collider clickableCollider;
    //public GameObject stationHighlight;

    private Highlights highlight;

    // If isEnable is false, then the player should not be able to reach to interact with this station/prop
    [SerializeField] private bool isEnabled;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled= true;
        if(clickableCollider!= null)
        {
            clickableCollider = GetComponent<Collider>();
        }
    }


}
