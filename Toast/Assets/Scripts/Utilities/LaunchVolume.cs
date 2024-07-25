/*
 * Launch Volume script - Nick Kannenberg
 * 
 * When attached to a trigger volume, can be used to launch objects within the volume
 * to a given position
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.VFX;

public class LaunchVolume : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private Vector3 targetPosition;

    [SerializeField]
    private bool directionalLaunch = false;
    [SerializeField]
    private float force = 0.0f;

    private BoxCollider launchCollider; // Must be attached to an object with a box collider

    private LayerMask mask;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        launchCollider = GetComponent<BoxCollider>();
        mask = LayerMask.GetMask("Interactable");
    } 

    public void LaunchItems()
    {
        Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(launchCollider.center), launchCollider.size / 2.0f, Quaternion.identity, mask);

        List<Rigidbody> launchedBodies = new List<Rigidbody>();
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null && !launchedBodies.Contains(rb))
            {
                if (!directionalLaunch)
                {
                    float targetX = (transform.TransformPoint(targetPosition) - hit.gameObject.transform.position).magnitude;
                    float vx = Mathf.Sqrt((Mathf.Abs(Physics.gravity.y) * targetX)/(Mathf.Sin(Mathf.Deg2Rad * 90.0f)));

                    Vector3 targetXVector = hit.gameObject.transform.position + new Vector3(targetX, 0, 0);
                    Vector3 targetVector = transform.TransformPoint(targetPosition) - hit.gameObject.transform.position;
                    targetVector.y = 0;
                    targetVector.Normalize();

                    float sinT = Mathf.Sin(Mathf.Deg2Rad * 45);
                    vx = vx * sinT;

                    Vector3 forceVector = targetVector * vx;
                    forceVector.y = vx;

                    rb.AddForce(forceVector, ForceMode.VelocityChange);

                    launchedBodies.Add(rb);
                }
                else
                {
                    Vector3 targetVector = transform.TransformPoint(targetPosition) - hit.gameObject.transform.position;
                    targetVector.Normalize();

                    Vector3 forceVector = targetVector * force;

                    rb.AddForce(forceVector, ForceMode.VelocityChange);
                    launchedBodies.Add(rb);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.TransformPoint(targetPosition), 0.1f);
        if (launchCollider != null)
        {
            Gizmos.DrawCube(launchCollider.center, launchCollider.size);
        }
        else
        {
            if(gameObject.GetComponent<BoxCollider>() != null)
            {
                launchCollider = gameObject.GetComponent<BoxCollider>();
                Gizmos.DrawCube(launchCollider.center, launchCollider.size);
            }
        }
    }
}
