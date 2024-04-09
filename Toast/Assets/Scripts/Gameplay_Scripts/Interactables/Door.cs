using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector3 minRot; // Closed
    public Vector3 maxRot; // Open

    // amount that the door has opened
    private float interpolateAmount;

    public float speed = 3;

    public bool isOpen;

    public GameObject rotator;
    Transform rotatorTransform;

    private void Awake()
    {
        if (rotator == null)
        {
            rotator = transform.parent.gameObject;
        }

        rotatorTransform = rotator.GetComponent<Transform>();
        //this.GetComponent<BoxCollider>().isTrigger = true;
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
            }
        }

        Vector3 newRotation = rotatorTransform.localEulerAngles;
        newRotation.x = Mathf.Lerp(minRot.x, maxRot.x, interpolateAmount);
        newRotation.y = Mathf.Lerp(minRot.y, maxRot.y, interpolateAmount);
        newRotation.z = Mathf.Lerp(minRot.z, maxRot.z, interpolateAmount);

        rotatorTransform.localEulerAngles = newRotation;
    }

    public void Open()
    {
        isOpen = true;
    }

    public void Close()
    {
        isOpen = false;
    }

    private void OnMouseDown()
    {
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
