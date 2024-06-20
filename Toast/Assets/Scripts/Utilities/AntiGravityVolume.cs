using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiGravityVolume : MonoBehaviour
{
    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// On trigger enter, disable gravity
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }

    /// <summary>
    /// On Trigger Exit, re-enable gravity
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {
        GameObject parent = other.gameObject;
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }
    }
}
