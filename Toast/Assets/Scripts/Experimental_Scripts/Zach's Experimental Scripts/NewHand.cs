using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField] protected GameObject handPos;
    [SerializeField] private GameObject heldObject;
    private List<GameObject> heldObjects = new List<GameObject>();

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent pickUpEvent;
    [SerializeField]
    private PropIntGameEvent dropEvent;

    [SerializeField]
    private InHandAttribute inHandAttribute;
    //[SerializeField]
    //private InHandAttribute mainHandAttribute;

    // ------------------------------- Functions -------------------------------

    // Uses the item in hand
    public bool TryUseInHand()
    {
        // If holding an item, get use strategy and Use it
        if (heldObject != null)
        {
            return heldObject.GetComponent<NewProp>().TryUse();
        }

        return false;
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
            if (heldObject.GetComponent<NewProp>().HasFlag(PropFlags.ImmuneToDrop))
            {
                return null;
            }

            // Set return
            itemToReturn = heldObject;

            // Set prop flogs
            heldObject.GetComponent<NewProp>()?.RemoveFlag(PropFlags.InHand);
            
            heldObject.GetComponent<NewProp>()?.RemoveAttribute(inHandAttribute);

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
            //Debug.Log(itemToPickup);
            //Debug.Log(itemToPickup.GetComponent<NewProp>());
            //Debug.Log(itemToPickup.GetComponent<NewProp>().HasAttribute(mainHandAttribute));
            // If clicked object in hand, remove it instead
            if (itemToPickup.GetComponent<NewProp>() != null && itemToPickup.GetComponent<NewProp>().HasAttribute(inHandAttribute))
            {
                Debug.Log("HELLO");
                itemToPickup.GetComponent<NewProp>().ForceRemoveFromHand();
            }
            
            // Inventory checks
            if (StationManager.instance.playerLocation.stationLabel == Stations.Inventory)
            {
                InventoryManager.instance.RemoveItemFromInventory(itemToPickup);
            }

            // Objective calls
            if (pickUpEvent != null)
                pickUpEvent.RaiseEvent(itemToPickup.GetComponent<NewProp>(), 1);

            // Set object transform
            itemToPickup.transform.parent = this.gameObject.transform;

            itemToPickup.transform.position = handPos.transform.position;
            itemToPickup.transform.rotation = handPos.transform.rotation;

            //heldObjects.Add(itemToPickup);
            heldObject = itemToPickup;

            // Set prop flags
            itemToPickup.GetComponent<NewProp>()?.AddFlag(PropFlags.InHand);

            itemToPickup.GetComponent<NewProp>()?.AddAttribute(inHandAttribute);
        }
    }
}
