using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------ Rotating Object ------------")]
    public GameObject rotator;
    private Transform rotatorTransform;

    [Header("------------ Rotation Values ------------")]
    public Vector3 minRot; // Closed
    public Vector3 maxRot; // Open
    public float speed = 3;

    [Header("------------ Bool Values ------------")]
    [SerializeField]
    private bool clickable;
    [SerializeField, ReadOnly]
    private bool isOpen;

    [Header("------------ Open/Close Events ------------")]
    [SerializeField]
    private bool hasEvents = false;
    [SerializeField, ShowIf("hasEvents")]
    private UnityEvent onOpen;
    [SerializeField, ShowIf("hasEvents")]
    private UnityEvent onClose;

    private bool lerping;

    [Header("Audio")]
    [SerializeField]
    private AudioEvent doorOpen;
    [SerializeField]
    private AudioEvent doorClose;

    private AudioSource source1;
    private AudioSource source2;

    [SerializeField, Button]
    private void setDoorOpen() { rotator.transform.localEulerAngles = maxRot; isOpen = true; }
    [SerializeField, Button]
    private void setDoorClosed() { rotator.transform.localEulerAngles = minRot; isOpen = false; }
    [SerializeField, Button]
    private void setMinToCurrentRotation() { minRot = rotator.transform.localEulerAngles; }
    [SerializeField, Button]
    private void setMaxToCurrentRotation() { maxRot = rotator.transform.localEulerAngles; }

    // amount that the door has opened
    private float interpolateAmount;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        if (rotator == null)
        {
            rotator = transform.parent.gameObject;
        }

        rotatorTransform = rotator.GetComponent<Transform>();
    }

    private void Start()
    {
        if(doorOpen != null)
        {
            source1 = gameObject.AddComponent<AudioSource>();
            source1.dopplerLevel = 0.0f;
            source1.spatialBlend = 1.0f;
        }

        if(doorClose != null)
        {
            source2 = gameObject.AddComponent<AudioSource>();
            source2.dopplerLevel = 0.0f;
            source2.spatialBlend = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            if (interpolateAmount >= 0 && interpolateAmount < 1)
            {
                interpolateAmount += Time.deltaTime * speed;
            }
            else
            {
                interpolateAmount = 1;
                if(lerping == true)
                {
                    lerping = false;
                    if(doorOpen != null)
                    {
                        doorOpen.Play(source1);
                    }
                }
            }
        }
        else
        {
            if (interpolateAmount > 0 && interpolateAmount <= 1)
            {
                interpolateAmount -= Time.deltaTime * speed;
            }
            else
            {
                interpolateAmount = 0;
                if (lerping == true)
                {
                    lerping = false;
                    if (doorClose != null)
                    {
                        doorClose.Play(source2);
                    }
                }
            }
        }

        Vector3 newRotation = rotatorTransform.localEulerAngles;
        newRotation.x = Mathf.Lerp(minRot.x, maxRot.x, interpolateAmount);
        newRotation.y = Mathf.Lerp(minRot.y, maxRot.y, interpolateAmount);
        newRotation.z = Mathf.Lerp(minRot.z, maxRot.z, interpolateAmount);

        rotatorTransform.localEulerAngles = newRotation;
    }

    // Opens the door
    public void Open()
    {
        isOpen = true;
        lerping = true;
        onOpen.Invoke();
    }

    // Closes the door
    public void Close()
    {
        isOpen = false;
        lerping = true;
        onClose.Invoke();
    }

    // On mouse down, toggle open - POTENTIALLY DESIRED ON CLICK FUNCTIONALITY
    private void OnMouseDown()
    {
        if (!clickable) return;

        if (!isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}
