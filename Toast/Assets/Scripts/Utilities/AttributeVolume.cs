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
using UnityEngine.UI;

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

    [SerializeField]
    private List<PropFlags> applyFlags = new List<PropFlags>();
    [SerializeField]
    private List<PropFlags> ignoreFlags = new List<PropFlags>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetAttributes(int i)
    {
        BoxCollider attCollider = gameObject.GetComponent<BoxCollider>();
        Vector3 r = transform.TransformVector(attCollider.size);
        r.x = Mathf.Abs(r.x/2f);
        r.y = Mathf.Abs(r.y / 2f);
        r.z = Mathf.Abs(r.z / 2f);
        Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(attCollider.center), r, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            CheckExit(collider.gameObject);
        }

        flagsToApply = applyFlags[i];

        foreach (Collider collider in colliders)
        {
            CheckEnter(collider.gameObject);
        }
    }

    public void SetIgnoreAttributes(int i)
    {
        BoxCollider attCollider = gameObject.GetComponent<BoxCollider>();
        Vector3 r = transform.TransformVector(attCollider.size);
        r.x = Mathf.Abs(r.x/2f);
        r.y = Mathf.Abs(r.y/2f);
        r.z = Mathf.Abs(r.z/2f);
        Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(attCollider.center), r, Quaternion.identity);

        foreach (Collider collider in colliders)
        {
            CheckExit(collider.gameObject);
        }

        flagsToIgnore = ignoreFlags[i];

        foreach (Collider collider in colliders)
        {
            CheckEnter(collider.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckEnter(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckExit(other.gameObject);
    }

    private void CheckExit(GameObject other)
    {
        NewProp prop = other.GetComponentInParent<NewProp>();

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

    private void CheckEnter(GameObject other)
    {
        NewProp prop = other.GetComponentInParent<NewProp>();

        if (prop != null)
        {
            // Break if has ignore flags
            if (flagsToIgnore != PropFlags.None && prop.HasFlag(flagsToIgnore)) { return; }
            // Break if has the flags that are being applied
            if (prop.HasFlag(flagsToApply)) { return; }


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

    private void OnDrawGizmosSelected()
    {
        BoxCollider c = GetComponent<BoxCollider>();
        Gizmos.color = new Color(0, 1, 0, 1);
        Gizmos.DrawCube(transform.TransformPoint(c.center), transform.TransformVector(c.size));
    }

}
