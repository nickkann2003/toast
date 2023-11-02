using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float minRotation = 180; // closed
    public float maxRotation = 80; // open

    // amount that the door has opened
    private float interpolateAmount;

    public float speed = 3;

    public bool isOpen;

    private void Awake()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
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

        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y = Mathf.Lerp(minRotation, maxRotation, interpolateAmount);

        transform.localEulerAngles = newRotation;
    }

    private void OnMouseDown()
    {
        if (!isOpen)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }
    }
}
