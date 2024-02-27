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
            //heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            

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
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.DropObject, itemToReturn.GetComponent<NewProp>().attributes, true));
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
            // if the object is already in a hand, remove it
            if (itemToPickup.GetComponent<NewProp>() != null && itemToPickup.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
            {
                Debug.Log("Forcibly removing from hand");
                itemToPickup.GetComponent<NewProp>().ForceRemoveFromHand();
            }

            heldObject = itemToPickup;
            heldObject.GetComponent<NewProp>()?.AddAttribute(PropFlags.InHand);
            _useStrategy = heldObject.GetComponent<IUseStrategy>();
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.PickUpObject, heldObject.GetComponent<NewProp>().attributes, true));
            heldObject.transform.parent = this.gameObject.transform;
            //if (this.transform.parent.GetComponent<Collider>())
            //{
            //    Physics.IgnoreCollision(this.transform.parent.GetComponent<Collider>(), heldObject.GetComponent<Collider>(), true);
            //}
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
