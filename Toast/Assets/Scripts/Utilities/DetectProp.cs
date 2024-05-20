using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectItem : MonoBehaviour
{
    [Header("Prop Variables")]
    [SerializeField]
    private PropFlags attributes;
    [SerializeField]
    private bool flagsAreCumulative = false;

    [Header("Completion Variables")]
    [SerializeField]
    private int numItems;
    private int curItems;
    [SerializeField]
    private float itemStayTime;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent completionEvent;

    [SerializeField]
    private bool eventOnStart;
    [EnableIf("eventOnStart")]
    [SerializeField]
    private UnityEvent startEvent;

    [SerializeField]
    private bool eventOnProgress;
    [EnableIf("eventOnProgress")]
    [SerializeField]
    private int progressValue;
    [SerializeField]
    private UnityEvent progressEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        NewProp otherProp = other.GetComponent<NewProp>();
        if (otherProp != null) 
        { 
            
        }
    }
}
