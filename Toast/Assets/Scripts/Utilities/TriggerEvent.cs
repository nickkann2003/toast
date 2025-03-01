using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public PropIntGameEvent e;

    public void Trigger()
    {
        e.RaiseEvent(null, 1);
    }
}
