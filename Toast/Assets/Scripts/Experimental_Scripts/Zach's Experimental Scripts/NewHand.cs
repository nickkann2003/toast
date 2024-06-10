using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField] protected GameObject handPos;
    private GameObject heldObject;
    private IUseStrategy _useStrategy;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent pickUpEvent;
    [SerializeField]
    private PropIntGameEvent dropEvent;

    // ------------------------------- Functions -------------------------------
    public void Update()
    {
        // Sets objects position every frame, if there is a held object
        if (heldObject != null)
        {
            heldObject.transform.position = handPos.transform.position;
            heldObject.transform.rotation = handPos.transform.rotation;            

            // If held item does not have hand flag, set it
            if (!heldObject.GetComponent<NewProp>().propFlags.HasFlag(PropFlags.InHand))
            {
                heldObject.GetComponent<NewProp>().AddAttribute(PropFlags.InHand);
            }
        }
    }

    // Uses the item in hand
    public void UseInHand()
    {
        // If holding an item, get use strategy and Use it
        if (heldObject != null)
        {
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
            if (_useStrategy != null)
            {
                _useStrategy.Use();
            }
            else if (heldObject.GetComponent<NewProp>() != null)
            {
                heldObject.GetComponent<NewProp>().Use();
            }
        }
    }

    // Returns the held object or null
    public GameObject CheckObject()
    { 
        return heldObject;
    }

    // Drops the current object at the current station
    public GameObject Drop()
    {
        // Item to be returned
        GameObject itemToReturn = null;

        // If holding an object, set prop flags and drop it
        if (heldObject != null)
        {
            // If item can't be dropped, don't drop it
            if (heldObject.GetComponent<NewProp>().HasAttribute(PropFlags.ImmuneToDrop))
            {
                return null;
            }

            // Set return
            itemToReturn = heldObject;

            // Set prop flogs
            heldObject.GetComponent<NewProp>()?.RemoveAttribute(PropFlags.InHand);
            
            // Check if dropping in inventory
            if (StationManager.instance.playerLocation.stationLabel == Stations.Inventory)
            {
                InventoryManager.instance.AddItemToInventory(itemToReturn);
            }

            // Update drop objectives
            if(dropEvent != null)
                dropEvent.RaiseEvent(heldObject.GetComponent<NewProp>(), 1);
            
            heldObject.transform.parent = null;
            heldObject = null;
        }

        return itemToReturn;
    }

    // Picks up a target object
    public void Pickup(GameObject itemToPickup)
    {
        // If item being picked up is not null
        if (itemToPickup != null)
        {
            // If clicked object in hand, remove it instead
            if (itemToPickup.GetComponent<NewProp>() != null && itemToPickup.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
            {
                itemToPickup.GetComponent<NewProp>().ForceRemoveFromHand();
            }

            // Otherwise, grab item reference
            heldObject = itemToPickup;

            // Set prop flags
            heldObject.GetComponent<NewProp>()?.AddAttribute(PropFlags.InHand);
            
            // Set use strategy
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
            
            // Inventory checks
            if (StationManager.instance.playerLocation.stationLabel == Stations.Inventory)
            {
                InventoryManager.instance.RemoveItemFromInventory(itemToPickup);
            }

            // Objective calls
            if (pickUpEvent != null)
                pickUpEvent.RaiseEvent(heldObject.GetComponent<NewProp>(), 1);
            

            
            
            // Set object transform
            heldObject.transform.parent = this.gameObject.transform;
        }
    }

    // Sets the current use strategy
    public void SwapUse(IUseStrategy strategy)
    {
        if (strategy != null)
        {
            _useStrategy = strategy;
        }
    }
}
