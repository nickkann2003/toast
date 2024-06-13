using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Hand : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------ Hand Visual Offset ------------")]
    public Vector3 offset = new Vector3(0.35f, -0.35f, 0.55f);

    [Header("------------ Particles ------------")]
    public ParticleSystem EatParticles;

    private Camera cam;
    private GameObject currentItem;
    private bool holdingItem;

    //TODO: BAD TEMP CODE, GET IT OUT
    private bool dropPressed = false;
    private bool eatPressed = false;

    // TEMP FIX ATTEMPT
    private float pickupCooldown = 0.1f;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent pickUpEvent;
    [SerializeField]
    private PropIntGameEvent dropEvent;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        holdingItem = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
        // If theres a held item, set its position to hand position
        if (currentItem != null)
        {
            currentItem.transform.position = cam.transform.position + (offset);
        }

        //TODO: BAD TEMP CODE, GET IT OUT
        if (Input.GetButtonDown("Use"))
        {
            if (!eatPressed)
            {
                eatPressed = true;
                EatHeldItem();
            }
        }
        else
        {
            eatPressed = false;
        }

        // Pickup pressed
        if (Input.GetButtonDown("Pickup"))
        {
            // If drop has not been pressed yet
            if (!dropPressed)
            {
                // If holding an item, set pickup cooldown
                if (holdingItem)
                {
                    pickupCooldown = 0.1f;
                }

                dropPressed = true;
                // Drop the item
                RemoveItem();
            }
        }
        else
        {
            dropPressed = false;
        }

        pickupCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// Adds an item to this hand
    /// </summary>
    /// <param name="item">Item to be added</param>
    public void AddItem(GameObject item)
    {
        if(pickupCooldown <= 0)
        {
            pickupCooldown = 0.1f;
            // If not holding anything, set internal values
            if (!holdingItem)
            {
                currentItem = item;
                holdingItem = true;
                pickUpEvent.RaiseEvent(currentItem.GetComponent<NewProp>(), 1);
            }
            else
            {
                RemoveItem();
            }
        }
        return;
    }

    // Remove the currently held item
    public GameObject RemoveItem()
    {
        // If currently holding an item
        if (holdingItem)
        {
            // Get the current station
            Station currentStation = StationManager.instance.playerLocation;

            // Grab world pos from the current station
            Vector3 objectPos = currentStation.ObjectOffset;

            // Set the current position of the object
            currentItem.transform.position = objectPos;

            // Try setting rigidbody, catch if object doesnt have a rigidbody
            try
            {
                currentItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            catch
            {
                Debug.Log("Dropped item without a rigid body");
            }

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
        if (mPos.x < cam.pixelWidth && mPos.x > cam.pixelWidth - 100 && mPos.y < 100 && mPos.y > 0)
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

    // Eat code
    public void EatHeldItem()
    {
        if (holdingItem)
        {
            if (currentItem != null)
            {
                if (currentItem.GetComponent<IEatable>() != null)
                {
                    PlayEatParticles(currentItem);
                    currentItem.GetComponent<IEatable>().TakeBite();
                    if (currentItem == null)
                    {
                        holdingItem = false;
                        currentItem = null;
                    }
                }
            }
            else
            {
                holdingItem = false;
            }
        }
    }

    /// <summary>
    /// Plays particle effects for eating
    /// </summary>
    /// <param name="item">Item that was eaten</param>
    public void PlayEatParticles(GameObject item)
    {
        Color c = item.GetComponent<Renderer>().material.color;
        var main = EatParticles.main;
        main.startColor = c;       
        EatParticles.Play();
    }
}
