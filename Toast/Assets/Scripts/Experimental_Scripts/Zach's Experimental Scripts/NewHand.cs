using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour, IUseStrategy
{
    [SerializeField] protected GameObject handPos;
    private GameObject heldObject;
    private IUseStrategy _useStrategy;

    public void Update()
    {
        heldObject.transform.position = handPos.transform.position;
    }

    public void Use(GameObject gameObject)
    {
        if (_useStrategy != null)
        {
            _useStrategy.Use(gameObject);
            return;
        }

        if (gameObject != null)
        {
            _useStrategy = gameObject.GetComponent<IUseStrategy>();
            _useStrategy?.Use(gameObject);
        }
    }

    public GameObject CheckObject()
    { 
        return heldObject; 
    }

    public GameObject SwapGameObjects(GameObject itemToPickup)
    {
        heldObject.GetComponent<NewProp>()?.RemoveAttribute(NewProp.PropFlags.InHand);
        GameObject itemToReturn = heldObject;

        heldObject = itemToPickup;
        heldObject.GetComponent<NewProp>()?.AddAttribute(NewProp.PropFlags.InHand);
        _useStrategy = heldObject.GetComponent<IUseStrategy>();

        return itemToReturn;
    }
}
