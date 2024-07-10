using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private float radius;

    [SerializeField]
    private float force;

    [SerializeField]
    private ForceMode forceMode;

    // ------------------------------- Functions -------------------------------
    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if(rb != null && rb.gameObject != gameObject)
        {
            rb.AddExplosionForce(-force, transform.position, radius, 0.0f, forceMode);
        }
    }

    public void ToggleMagnet()
    {
        if (this.enabled)
        {
            this.enabled = false;
        }
        else
        {
            this .enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
