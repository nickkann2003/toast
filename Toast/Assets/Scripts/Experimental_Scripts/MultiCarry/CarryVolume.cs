using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryVolume : MonoBehaviour
{
    [SerializeField]
    Carrier carrierObject;


    public List<GameObject> currentCarries;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        currentCarries = new List<GameObject>();
        if (carrierObject == null)
        {
            carrierObject = gameObject.GetComponentInParent<Carrier>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!carrierObject.isHeld)
        {
            // Only include interactable objects that aren't the parent
            if (other.GetComponent<NewProp>())
            {
                currentCarries.Add(other.gameObject);
                other.transform.SetParent(carrierObject.gameObject.transform);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!carrierObject.isHeld)
        {
           currentCarries.Remove(other.gameObject);
           other.transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
