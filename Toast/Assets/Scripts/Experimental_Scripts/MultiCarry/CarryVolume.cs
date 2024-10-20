using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryVolume : MonoBehaviour
{
    [SerializeField]
    Carrier carrierObject;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        if(carrierObject == null)
        {
            carrierObject = gameObject.GetComponentInParent<Carrier>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!carrierObject.isHeld)
        {
            // Only include interactable objects that aren't the parent
            if (other.gameObject.layer == 7 && other.gameObject != transform.parent)
            {
                carrierObject.currentCarries.Add(other.gameObject);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        /*
        if (!carrierObject.isHeld)
        {
            carrierObject.currentCarries.Remove(other.gameObject);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
