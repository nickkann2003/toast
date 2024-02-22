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
            if ((bool)heldObject.GetComponent<NewProp>()?.attributes.HasFlag(PropFlags.Giant))
            {
                transform.localScale *= 2f;
            }
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
            if ((bool)heldObject.GetComponent<NewProp>()?.attributes.HasFlag(PropFlags.Giant))
            {
                transform.localScale *= .5f;
            }
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
