using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility script, allows an object to be toggled Active and Inactivate based on its current state
/// </summary>
public class ActiveToggle : MonoBehaviour
{
    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
