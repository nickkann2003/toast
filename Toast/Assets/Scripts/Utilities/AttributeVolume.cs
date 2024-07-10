/*
 * Attribute Volume script - Nick Kannenberg
 * 
 * Applies an attribute to NewProps that enter the volume
 * Optionally removes that attribute upon leaving the volume
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AttributeVolume : MonoBehaviour
{
    // List of props in this trigger that were affected
    private List<NewProp> props = new List<NewProp>();
    [SerializeField] // Flags to apply to objects entering
    private PropFlags flagsToApply = PropFlags.None;
    [SerializeField] // Flags to ignore on objects
    private PropFlags flagsToIgnore = PropFlags.None;

    [SerializeField] // Should flags be removed on exit
    private bool removeOnExit = false;

    [SerializeField]
    private bool destroyNonIgnoreObjects = false; // Destroys objects that enter that do not match the ignore flags, instead of adding them to the list

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger");
        NewProp prop = other.GetComponent<NewProp>();
        if (prop != null)
        {
            Debug.Log("Prop script");

            // Break if has ignore flags
            if (flagsToIgnore != PropFlags.None && prop.HasFlag(flagsToIgnore)) { return; }
            // Break if has the flags that are being applied
            if(prop.HasFlag(flagsToApply)) { return; }


            if (!props.Contains(prop))
            {
                // If destroying non-ignore, destroy it instead
                if (destroyNonIgnoreObjects)
                {
                    Destroy(other.gameObject);
                    return;
                }

                props.Add(prop);

                // Give prop the attributes
                prop.propFlags |= flagsToApply;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NewProp prop = other.GetComponent<NewProp>();
        if (prop != null)
        {
            if (props.Contains(prop))
            {
                props.Remove(prop);

                if (removeOnExit)
                {
                    // Remove the attributes from the prop
                    prop.propFlags ^= flagsToApply;
                }
            }
        }
    }
}
