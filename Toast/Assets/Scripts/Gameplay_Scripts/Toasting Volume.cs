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
    public float toastRate = 0.2f;
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
                prop.IncreaseToastiness(toastRate * Time.deltaTime);
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
