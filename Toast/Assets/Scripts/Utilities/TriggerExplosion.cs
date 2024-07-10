using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExplosion : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private float radius;

    [SerializeField] 
    private float upwardsMod = 1.0f;

    [SerializeField]
    [InfoBox("1 - 10 is a shake, 30+ is a mild explosion, 100+ is a major explosion, when referencing bread")]
    private float explosionPower;

    [SerializeField]
    private ForceMode explosionMode;

    [SerializeField, Button]
    private void TriggerExplode() { Explode(); }

    // ------------------------------- Functions -------------------------------
    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(explosionPower, transform.position, radius, upwardsMod, explosionMode);
        }
    }

    public void ExplodeAtLocation(Vector3 location)
    {
        transform.position = location;
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
