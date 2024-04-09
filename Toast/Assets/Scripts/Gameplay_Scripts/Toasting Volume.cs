using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/*
 * Toast Volume class
 * Toasts things within a given volume while turned on
 */
public class ToastingVolume : MonoBehaviour
{
    // Variables --------------------------
    public float toastRate = 1.0f;
    public Dial toastingValueDial;
    public BoxCollider volume;
    public UnityEvent onToastingStart;
    public UnityEvent onToastingStop;

    private bool toasting;
    private List<NewProp> toastingObjects = new List<NewProp>();

    // Functions --------------------------
    private void Update()
    {
        if (toasting)
        {
            foreach(NewProp prop in toastingObjects)
            {
                prop.toastiness += toastRate * Time.deltaTime;
                if (prop != null)
                {
                    if (prop.toastiness > .15f && !prop.attributes.HasFlag(PropFlags.Toast))
                    {
                        prop.AddAttribute(PropFlags.Toast);
                        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, prop.attributes, true));
                    }

                    if (prop.toastiness > .9f && !prop.attributes.HasFlag(PropFlags.Burnt))
                    {
                        prop.AddAttribute(PropFlags.Burnt);
                        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, prop.attributes, true));
                    }
                }
            }
        }
    }

    public void StartToasting()
    {
        toasting = true;
        onToastingStart.Invoke();
    }

    public void StopToasting()
    {
        toasting = false;
        onToastingStop.Invoke();
    }

    public void SetToastingValue(int toastValue)
    {
        toastRate = toastValue;
        if (toastRate == 0)
        {
            StopToasting();
        }
        else
        {
            StartToasting();
        }
    }

    public void CheckToastingValue()
    {
        if(toastingValueDial != null)
        {
            toastRate = toastingValueDial.dialValue;
            if (toastRate == 0)
            {
                StopToasting();
            }
            else
            {
                StartToasting();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        NewProp prop = other.gameObject.GetComponent<NewProp>();
        if (prop != null)
        {
            toastingObjects.Add(prop);
        }
    }

    void OnTriggerExit(Collider other)
    {
        NewProp prop = other.gameObject.GetComponent<NewProp>();
        if (prop != null)
        {
            toastingObjects.Remove(prop);
        }
    }
}
