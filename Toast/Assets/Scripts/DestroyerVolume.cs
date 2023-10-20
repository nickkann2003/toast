using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerVolume : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        while(parent.transform.parent != null)
        {
            parent = parent.transform.parent.gameObject;
        }
        Destroy(parent);
    }
}
