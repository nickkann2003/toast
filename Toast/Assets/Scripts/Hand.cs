using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Variables -----------------------------------------------
    private Camera cam;
    private GameObject currentItem;
    private bool holdingItem;
    public Vector3 offset = new Vector3(0.35f, -0.35f, 0.55f);

    // Start is called before the first frame update -----------
    void Start()
    {
        holdingItem = false;
        cam = Camera.main;
    }

    // Update is called once per frame -------------------------
    void Update()
    {
        // If theres a held item, set its position to hand position
        if(currentItem != null)
        {
            currentItem.transform.position = cam.transform.position + (offset);
        }
    }

    // Functions ----------------------------------------
    public void AddItem(GameObject item)
    {
        // If not holding anything, set internal values
        if(!holdingItem)
        {
            currentItem = item;
            holdingItem = true;
        }
        return;
    }

    // Remove the currently held item
    public GameObject RemoveItem()
    {
        // If holding item, unset values and return the item
        if (holdingItem)
        {
            GameObject item = currentItem;
            currentItem = null;
            holdingItem = false;
            return item;
        }
        return null;
    }

    // TODO: Lock will need to be changed when camera rotates
    // Overload for passing a Z-Lock when removing an item
    public GameObject RemoveItem(float zOffset)
    {
        // Actual locked world pos
        float zLock = cam.transform.position.z + zOffset;
        if (holdingItem)
        {
            Vector3 lockPos = new Vector3(currentItem.transform.position.x, currentItem.transform.position.y, zLock);
            currentItem.transform.position = lockPos;
            GameObject item = currentItem;
            currentItem = null;
            holdingItem = false;
            return item;
        }
        return null;
    }

    // Checks if the mouse is in the drop position on the screen
    public bool CheckDrop()
    {
        Vector2 mPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Debug.Log("MPos: " + mPos + ", Width: " + cam.pixelWidth + ", Height: " + cam.pixelHeight);
        if(mPos.x < cam.pixelWidth && mPos.x > cam.pixelWidth-100 && mPos.y < 100 && mPos.y > 0)
        {
            return true;
        }
        return false;
    }

    // Check if a given item is the held item
    public bool CheckHeldItem(GameObject item)
    {
        if (holdingItem)
        {
            if (currentItem.Equals(item))
            {
                return true;
            }
        }
        return false;
    }
}