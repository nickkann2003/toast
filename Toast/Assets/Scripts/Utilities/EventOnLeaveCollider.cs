using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnLeaveCollider : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onLeaveEvent;

    private void OnTriggerExit(Collider other)
    {
        onLeaveEvent.Invoke();
    }
}
