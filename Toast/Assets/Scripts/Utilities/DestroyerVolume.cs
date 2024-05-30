using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerVolume : MonoBehaviour
{
    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// On trigger enter, destroy other and send event
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        RequirementEvent rEvent;
        PropFlags flags = PropFlags.None;
        // Check if object has new prop
        if(other.gameObject.GetComponent<NewProp>() != null)
        {
            parent = other.gameObject;
            flags = parent.GetComponent<NewProp>().attributes;
            rEvent = new RequirementEvent(RequirementType.DestroyObject, flags, true);
            ObjectiveManager.instance.UpdateObjectives(rEvent);
            Destroy(parent);
            return;
        }

        // Check if its parent has new prop
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
