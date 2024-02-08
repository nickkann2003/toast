using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour, IUse
{
    private GameObject heldObject;
    private IUse _useHeld;

    public void Use(GameObject gameObject)
    {
        if (_useHeld != null)
        {
            _useHeld.Use(gameObject);
            return;
        }

        if (gameObject != null)
        {
            _useHeld = gameObject.GetComponent<IUse>();
            if ( _useHeld != null )
            {
                _useHeld.Use(gameObject);
            }
        }
    }

    public GameObject SwapGameObjects(GameObject itemToPickup)
    {
        GameObject itemToReturn = heldObject;

        heldObject = itemToPickup;
        _useHeld = heldObject.GetComponent<IUse>();

        return itemToReturn;
    }
}
