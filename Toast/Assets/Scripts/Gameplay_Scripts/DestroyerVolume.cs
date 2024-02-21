using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerVolume : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        RequirementEvent rEvent;
        PropFlags flags = PropFlags.None;
        if (parent.GetComponent<NewProp>() != null)
        {
            flags = parent.GetComponent<NewProp>().attributes;
        }
        while (parent.transform.parent != null)
        {
            parent = parent.transform.parent.gameObject;
            if (parent.GetComponent<NewProp>() != null)
            {
                flags = parent.GetComponent<NewProp>().attributes;
            }
        }
        rEvent = new RequirementEvent(RequirementType.DestroyObject, flags, true);
        ObjectiveManager.instance.UpdateObjectives(rEvent);
        Destroy(parent);
    }
}
