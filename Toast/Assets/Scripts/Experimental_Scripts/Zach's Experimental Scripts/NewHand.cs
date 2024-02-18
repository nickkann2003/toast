using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    [SerializeField] protected GameObject handPos;
    private GameObject heldObject;
    private IUseStrategy _useStrategy;

    public void Update()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = handPos.transform.position;
        }
    }

    public void UseInHand()
    {
        if (heldObject != null)
        {
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
            _useStrategy?.Use();
        }
    }

    public GameObject CheckObject()
    { 
        return heldObject; 
    }

    public GameObject Drop()
    {
        GameObject itemToReturn = null;
        if (heldObject != null)
        {
            heldObject.GetComponent<NewProp>()?.RemoveAttribute(NewProp.PropFlags.InHand);
            itemToReturn = heldObject;
            itemToReturn.transform.localScale *= 2f; // TEMP MAKE HAND A SEPARATE CAM THAT OVERLAYS
            Debug.Log("Returning Held Object");
            heldObject = null;
        }

        return itemToReturn;
    }

    public void Pickup(GameObject itemToPickup)
    {
        if (itemToPickup != null)
        {
            heldObject = itemToPickup;
            itemToPickup.transform.localScale *= .5f; // TEMP MAKE HAND A SEPARATE CAM THAT OVERLAYS
            heldObject.GetComponent<NewProp>()?.AddAttribute(NewProp.PropFlags.InHand);
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
        }
    }

    public void SwapUse(IUseStrategy strategy)
    {
        if (strategy != null)
        {
            _useStrategy = strategy;
        }
    }
}
