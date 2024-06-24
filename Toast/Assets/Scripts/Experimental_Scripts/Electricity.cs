using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");

        if(collision.gameObject.TryGetComponent(out NewProp other))
        {
            if(other.attributes.HasFlag(PropFlags.Metal))
            {
                Debug.Log("Electricity");
            }
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");

        if (other.gameObject.TryGetComponent(out NewProp prop))
        {
            if (prop.attributes.HasFlag(PropFlags.Metal))
            {
                Debug.Log("Electricity");
            }
        }
    }
}
