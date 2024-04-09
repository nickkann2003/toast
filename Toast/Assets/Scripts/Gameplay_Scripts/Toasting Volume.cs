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
    public BoxCollider volume;
    public UnityEvent onToastingStart;
    public UnityEvent onToastingStop;

    private bool toasting;
    private List<NewProp> toastingObjects;

    // Functions --------------------------
    private void Update()
    {
        if (toasting)
        {
            foreach(NewProp obj in toastingObjects)
            {
                obj.toastiness += toastRate * Time.deltaTime;
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
