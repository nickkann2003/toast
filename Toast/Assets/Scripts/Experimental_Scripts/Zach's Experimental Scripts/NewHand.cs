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
            heldObject.transform.rotation = handPos.transform.rotation;
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            if (!heldObject.GetComponent<NewProp>().attributes.HasFlag(PropFlags.InHand))
            {
                heldObject.GetComponent<NewProp>().AddAttribute(PropFlags.InHand);
            }
        }
    }

    public void UseInHand()
    {
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
            heldObject.transform.parent = null;
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
            heldObject.transform.parent = this.transform;
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
