using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Drawer : MonoBehaviour
{

    // ------------------------------- Variables -------------------------------
    [Header("------------ Moving Object ------------")]
    public GameObject objectToMove;

    [Header("------------ Rotation Values ------------")]
    public Vector3 minPos; // Closed
    public Vector3 maxPos; // Open
    public float speed = 3;
    public bool isOpen;

    // amount that the door has opened
    private float interpolateAmount;

    [SerializeField, Button]
    private void setDrawerOpen() { objectToMove.transform.localPosition = maxPos; isOpen = true; }
    [SerializeField, Button]
    private void setDrawerClosed() { objectToMove.transform.localPosition = minPos; isOpen = false; }
    [SerializeField, Button]
    private void setMinToCurrentPosition() { minPos = transform.localPosition; }
    [SerializeField, Button]
    private void setMaxToCurrentPosition() { maxPos = transform.localPosition;  }

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        if (objectToMove == null)
        {
            objectToMove = transform.parent.gameObject;
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

        Vector3 newPosition = objectToMove.transform.localPosition;
        newPosition.x = Mathf.Lerp(minPos.x, maxPos.x, interpolateAmount);
        newPosition.y = Mathf.Lerp(minPos.y, maxPos.y, interpolateAmount);
        newPosition.z = Mathf.Lerp(minPos.z, maxPos.z, interpolateAmount);

        objectToMove.transform.localPosition = newPosition;
    }

    // Opens the drawer
    public void Open()
    {
        isOpen = true;
    }

    // Closes the drawer
    public void Close()
    {
        isOpen = false;
    }

    //// On mouse down, toggle open - POTENTIALLY DESIRED ON CLICK FUNCTIONALITY
    //private void OnMouseDown()
    //{
    //    if (!isOpen)
    //    {
    //        Open();
    //    }
    //    else
    //    {
    //        Close();
    //    }
    //}
}
