using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class NewProp : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------ Attributes ------------")]
    public PropFlags attributes;

    // Toast Values
    [Header("------------ Toastiness ------------")]
    public float toastiness;

    [Header("------------ Fire Variables ------------")]
    public float fireTrigger = 1.5f;
    public GameObject firePrefab;
    private float targetStrength = 0.5f;
    private Color strongestStrength = Color.black;
    private Color initialColor;
    private Color colorOffset;

    [Header("------------ Freeze Variables ------------")]
    public float frozenness;

    [SerializeField]
    private Material baseMat;

    protected IUseStrategy _useStrategy;

    private Rigidbody _rigidbody;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Get use strategy on start
        _useStrategy = this.gameObject.GetComponent<IUseStrategy>();

        // Grab initial color and set color variables
        initialColor = gameObject.GetComponent<Renderer>().material.color;
        colorOffset = strongestStrength - initialColor;
        
        // If fire prefab not given, get it from manager
        if(firePrefab == null)
        {
            firePrefab = FireEndingManager.instance.firePrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If in hand and has a rigid body, destroy rigidbody
        if (attributes.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.GetComponent<Rigidbody>());
        }

        // If not in hand and doesn't have a rigid body, give it a rigidbody
        if (!attributes.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
    }

    // Use this prop's strategy
    public virtual void Use()
    {
        _useStrategy?.Use();
    }

    // Force removes this item from hand
    public void ForceRemoveFromHand()
    {
        // If this item is in hand, then force hand to drop it
        if (HasAttribute(PropFlags.InHand))
        {
            transform.parent.GetComponent<NewHand>()?.Drop();

            // If no rigidbody when dropped, get one
            if (this.GetComponent<Rigidbody>() == null)
            {
                this.AddComponent<Rigidbody>();
            }
        }
    }

    // Add an attribute to this prop
    public void AddAttribute(PropFlags flagToAdd)
    {
        attributes |= flagToAdd;
    }

    // Remove an attribute from this prop
    public void RemoveAttribute(PropFlags flagToRemove)
    {
        attributes &= ~flagToRemove;
    }

    // Check if a prop has attributes
    public bool HasAttribute(PropFlags flagToCheck)
    {
        return attributes.HasFlag(flagToCheck);
    }

    // Increase toastiness by a given value
    public void IncreaseToastiness(float val)
    {
        // Increase toastiness
        toastiness += val;

        // Get color strength and cap it
        float colorStrength = toastiness;
        if (colorStrength > 1)
        {
            colorStrength = 1;
        }

        // Set renderer color
        gameObject.GetComponent<Renderer>().material.color = initialColor + (colorOffset * colorStrength);

        // Adjust prop flags and trigger requirement events
        if (toastiness > .15f && !attributes.HasFlag(PropFlags.Toast)) // Toasted event
        {
            // Add attribtue
            AddAttribute(PropFlags.Toast);

            // Trigger Objectives
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
        }

        if (toastiness > .9f && !attributes.HasFlag(PropFlags.Burnt)) // Burnt event
        {
            // Add attributes
            AddAttribute(PropFlags.Burnt);

            // Trigger Objectives
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
        }
        if (!attributes.HasFlag(PropFlags.OnFire) && toastiness > fireTrigger && firePrefab != null) // On Fire event
        {
            // Instantiate fire
            GameObject fire = Instantiate(firePrefab);
            fire.transform.parent = gameObject.transform;
            fire.transform.localPosition = Vector3.zero;
            fire.transform.eulerAngles = Vector3.zero;
            fire.transform.localScale = Vector3.one;

            // Add attribute
            AddAttribute(PropFlags.OnFire);

            // Trigger objectives
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
            
            // Add flaming object
            FireEndingManager.instance.addFireObject(gameObject);
        }
    }

    // Defrosts this object
    public void DefrostToast()
    {
        // If frozen, destroy top game object and remove frozen flag
        if (attributes.HasFlag(PropFlags.Frozen))
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            attributes &= ~PropFlags.Frozen;
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.ThawObject, attributes, true));
        }
    }
}
