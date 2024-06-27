using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerVolume : MonoBehaviour
{
    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent destroyEvent;
    // ------------------------------- Functions -------------------------------
    private void Start()
    {
        if(destroyEvent == null)
        {
            destroyEvent = PieManager.instance.DestroyObjects;
        }
    }

    /// <summary>
    /// On trigger enter, destroy other and send event
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        // Check if object has new prop
        parent = other.gameObject;
        if(parent.GetComponent<NewProp>() != null)
        {
            destroyEvent.RaiseEvent(parent.GetComponent<NewProp>(), 1);
            Destroy(parent);
            return;
        }

        while (parent.transform.parent != null && parent.GetComponent<NewProp>() == null)
        {
            parent = parent.transform.parent.gameObject;
        }
        destroyEvent.RaiseEvent(parent.GetComponent<NewProp>(), 1);
        Destroy(parent);
    }
}
