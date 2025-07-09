/*
 * Author: Nick Kannenberg
 * 
 * Script for triggering a UnityEvent when any key is pressed, while this script is active
 * 
 */

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnAnyKeyPress : MonoBehaviour
{
    // Unity event to be triggered
    [SerializeField] private UnityEvent onKeyEvent;

    // True if any key should trigger event
    [SerializeField] private bool specificKeys = false;

    // If looking for specific keys, check this list
    [SerializeField, ShowIf("specificKeys")] private List<KeyCode> keys;

    // On update, check if a key is down, and optionally if its a specified key
    void Update()
    {
        if (gameObject.activeSelf)
        {
            // If any key is pressed
            if(Input.anyKey)
            {
                if(!specificKeys)
                {
                    // Invoke if any key allowed
                    onKeyEvent.Invoke();
                }
                else
                {
                    // Otherwise check all keycodes in list
                    foreach(KeyCode keyCode in keys)
                    {
                        if(Input.GetKey(keyCode))
                        {
                            onKeyEvent.Invoke();
                            return;
                        }
                    }
                }
            }
        }
    }
}
