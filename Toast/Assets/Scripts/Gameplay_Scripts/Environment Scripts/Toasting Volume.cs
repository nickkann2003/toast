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
    // ------------------------------- Variables -------------------------------
    [Header("Object References")]
    public Dial toastingValueDial;
    public BoxCollider volume;

    [Header("Start/Stop Events")]
    public UnityEvent onToastingStart;
    public UnityEvent onToastingStop;

    // Complex Heating Variables
    [Header ("Heating Values")]
    public float toastRate = 0.2f;
    public float heatingPower; // Determines how fast the volume heats up
    private float heat; // Degrees

    private bool toasting;
    private List<NewProp> toRemove = new List<NewProp>();
    private List<NewProp> toastingObjects = new List<NewProp>();

    // ------------------------------- Functions -------------------------------
    private void Update()
    {
        // If toasting, loop toasting objects
        if (toasting)
        {
            for(int i = 0; i < toastingObjects.Count; i ++)
            {
                // Get prop, check null
                NewProp prop = toastingObjects[i];
                if(prop == null)
                {
                    toRemove.Add(toastingObjects[i]);
                    continue;
                }
                if(prop.gameObject == null)
                {
                    toRemove.Add(toastingObjects[i]);
                    continue;
                }
                if (prop.gameObject.GetComponent<Rigidbody>() == null)
                {
                    toRemove.Add(toastingObjects[i]);
                    continue;
                }

                // Increase tostiness
                prop.IncreaseToastiness(toastRate * Time.deltaTime);
            }
        }

        // Remove outside of loop
        if(toRemove.Count > 0)
        {
            foreach(NewProp prop in toRemove)
            {
                toastingObjects.Remove(prop);
            }
            toRemove.Clear();
        }
    }

    // Starting toasting
    public void StartToasting()
    {
        toasting = true;
        onToastingStart.Invoke();
    }

    // Stop toasting
    public void StopToasting()
    {
        toasting = false;
        onToastingStop.Invoke();
    }

    // Set the current toasting value
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

    // Checks dial reference to determine current toasting setting
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

    // On enter trigger, add to toasting list
    void OnTriggerEnter(Collider other)
    {
        NewProp prop = other.gameObject.GetComponent<NewProp>();
        if (prop != null)
        {
            toastingObjects.Add(prop);
        }
    }

    // On exit trigger, remove from toasting list
    void OnTriggerExit(Collider other)
    {
        NewProp prop = other.gameObject.GetComponent<NewProp>();
        if (prop != null)
        {
            toastingObjects.Remove(prop);
        }
    }
}
