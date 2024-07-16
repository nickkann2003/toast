using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBlade : MonoBehaviour
{
    private Vector3 lastPos;
    private float speed;

    // ------------------------------- Functions -------------------------------
    private void OnEnable()
    {
        lastPos = transform.position;
    }

    private void Update()
    {
        speed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TN_Object>() != null)
        {
            other.gameObject.GetComponent<TN_Object>().Slice(this.transform.position, other.transform.position - lastPos, speed);
            other.gameObject.GetComponent<TN_Object>().Use();
        }

        //GameObject parent = other.gameObject;
        //RequirementEvent rEvent;
        //PropFlags flags = PropFlags.None;
        //if (parent.GetComponent<NewProp>() != null)
        //{
        //    flags = parent.GetComponent<NewProp>().attributes;
        //}
        //while (parent.transform.parent != null)
        //{
        //    parent = parent.transform.parent.gameObject;
        //    if (parent.GetComponent<NewProp>() != null)
        //    {
        //        flags = parent.GetComponent<NewProp>().attributes;
        //    }
        //}
        //rEvent = new RequirementEvent(RequirementType.DestroyObject, flags, true);
        //ObjectiveManager.instance.UpdateObjectives(rEvent);
        //Destroy(parent);
    }
}
