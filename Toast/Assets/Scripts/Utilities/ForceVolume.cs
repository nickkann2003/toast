/*
 * Force Volume script - Nick Kannenberg
 * Applies a constant force to all rigidbodies within the volume
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceVolume : MonoBehaviour
{
    [SerializeField, Header("Force of 2 floats bread. Acceleration of 10 floats all items")]
    private Vector3 forceToApply = Vector3.zero;

    [SerializeField, Header("Accelerate if you want all objects to act the same. Force if you want heavier objects less affected.")]
    private bool useAcceleration = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            if (!useAcceleration)
            {
                rb.AddForce(forceToApply, ForceMode.Force);

            }
            else
            {
                rb.AddForce(forceToApply, ForceMode.Acceleration);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
        if (useAcceleration)
        {
            Gizmos.DrawLine(transform.position, transform.position + (forceToApply * 0.1f));
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + (forceToApply * 0.3f));
        }
    }
}
