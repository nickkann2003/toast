using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    [SerializeField]
    List<ParticleSystem> particles;

    [SerializeField]
    ParticleSystem burstP; 

    // Start is called before the first frame update
    void Start()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Stop();
        }
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
        if (other.gameObject.TryGetComponent(out NewProp prop))
        {
            if (prop.attributes.HasFlag(PropFlags.Metal))
            {
                // Play visual
                foreach(ParticleSystem p in particles)
                {
    
                    p.Play();
                    
                }
            }
        }
    }
}
