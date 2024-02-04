using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerVolume : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        GameObject parent = other.gameObject;
        RequirementEvent rEvent;
        ObjectVariables objVars = null;
        if (parent.GetComponent<ObjectVariables>() != null)
        {
            objVars = parent.GetComponent<ObjectVariables>();
        }
        while (parent.transform.parent != null)
        {
            parent = parent.transform.parent.gameObject;
            if (parent.GetComponent<ObjectVariables>() != null)
            {
                objVars = parent.GetComponent<ObjectVariables>();
            }
        }
        if (objVars != null)
        {
            rEvent = new RequirementEvent(RequirementType.DestroyObject, objVars, true);
        }
        else
        {
            rEvent = new RequirementEvent(RequirementType.DestroyObject, new ObjectVariables(), true);
        }
        ObjectiveManager.instance.UpdateObjectives(rEvent);
        Destroy(parent);
    }
}
