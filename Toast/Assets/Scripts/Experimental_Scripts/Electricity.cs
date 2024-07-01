using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    [SerializeField]
    List<ParticleSystem> particles;

    [SerializeField]
    float radius, power;

    [SerializeField]
    Station endingStation;

    LayerMask mask;


    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Interactable");
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

                TriggerExplosion();
            }
        }
    }

    private void TriggerExplosion()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, mask);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

        Debug.Log(colliders.Length + "Colliders detected");

        StationManager.instance.MoveToStation(endingStation);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
