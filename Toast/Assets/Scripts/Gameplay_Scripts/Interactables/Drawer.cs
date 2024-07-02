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
    private bool lerping = false;

    private List<GameObject> inDrawer = new List<GameObject>();

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
                foreach (GameObject g in inDrawer)
                {
                    if(g != null)
                    {
                        if (g.GetComponent<Rigidbody>() != null)
                        {
                            g.GetComponent<Rigidbody>().AddForce((minPos - maxPos) * 25f);
                        }
                    }
                }
            }
            else
            {
                interpolateAmount = 1;
                if (lerping)
                {
                    lerping = false;
                }
            }
        }
        else
        {
            if (interpolateAmount > 0 && interpolateAmount <= 1)
            {
                interpolateAmount -= Time.deltaTime * speed;
                foreach (GameObject g in inDrawer)
                {
                    if(g != null)
                    {
                        if (g.GetComponent<Rigidbody>() != null)
                        {
                            g.GetComponent<Rigidbody>().AddForce((maxPos - minPos) * 25f);
                        }
                    }
                }
            }
            else
            {
                interpolateAmount = 0;
                if (lerping)
                {
                    lerping = false;
                }
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
        lerping = false;
        foreach (GameObject g in inDrawer)
        {
            if(g != null)
            {
                if (g.GetComponent<Rigidbody>() != null)
                {
                    g.GetComponent<Rigidbody>().AddForce((minPos - maxPos) * 100f);
                }
            }
        }
    }

    // Closes the drawer
    public void Close()
    {
        isOpen = false;
        lerping = true;
        foreach (GameObject g in inDrawer)
        {
            if(g != null)
            {
                if (g.GetComponent<Rigidbody>() != null)
                {
                    g.GetComponent<Rigidbody>().AddForce((maxPos - minPos) * 100f);
                }
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (!inDrawer.Contains(other.gameObject))
        {
            inDrawer.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inDrawer.Contains(other.gameObject))
        {
            inDrawer.Remove(other.gameObject);
        }
    }
}
