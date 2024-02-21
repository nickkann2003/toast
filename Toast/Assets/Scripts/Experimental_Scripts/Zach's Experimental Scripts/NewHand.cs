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
            heldObject.transform.localRotation = handPos.transform.localRotation;
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
            heldObject.GetComponent<NewProp>()?.RemoveAttribute(PropFlags.InHand);
            itemToReturn = heldObject;
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
            heldObject.GetComponent<NewProp>()?.AddAttribute(PropFlags.InHand);
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.PickUpObject, heldObject.GetComponent<NewProp>().attributes, true));
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
