using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class NewProp : MonoBehaviour
{
    [SerializeField]
    private Material baseMat;

    public PropFlags attributes;

    // Toast Values
    public float toastiness;
    public float fireTrigger = 1.5f;
    public GameObject firePrefab;
    private float targetStrength = 0.5f;
    private Color strongestStrength = Color.black;
    private Color initialColor;
    private Color colorOffset;

    public float frozenness;

    protected IUseStrategy _useStrategy;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _useStrategy = this.gameObject.GetComponent<IUseStrategy>();

        initialColor = gameObject.GetComponent<Renderer>().material.color;
        colorOffset = strongestStrength - initialColor;
        if(firePrefab == null)
        {
            firePrefab = FireEndingManager.instance.firePrefab;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attributes.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.GetComponent<Rigidbody>());
        }
        if (!attributes.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
    }

    public virtual void Use()
    {
        _useStrategy?.Use();
    }

    public void ForceRemoveFromHand()
    {
        if (HasAttribute(PropFlags.InHand))
        {
            transform.parent.GetComponent<NewHand>()?.Drop();
            RemoveAttribute(PropFlags.InHand);
            if (this.GetComponent<Rigidbody>() == null)
            {
                this.AddComponent<Rigidbody>();
            }
        }
    }

    public void AddAttribute(PropFlags flagToAdd)
    {
        attributes |= flagToAdd;
    }

    public void RemoveAttribute(PropFlags flagToRemove)
    {
        attributes &= ~flagToRemove;
    }

    public bool HasAttribute(PropFlags flagToCheck)
    {
        return attributes.HasFlag(flagToCheck);
    }

    public void IncreaseToastiness(float val)
    {
        toastiness += val;
        float colorStrength = toastiness;
        if (colorStrength > 1)
        {
            colorStrength = 1;
        }

        gameObject.GetComponent<Renderer>().material.color = initialColor + (colorOffset * colorStrength);

        if (toastiness > .15f && !attributes.HasFlag(PropFlags.Toast))
        {
            AddAttribute(PropFlags.Toast);
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
        }

        if (toastiness > .9f && !attributes.HasFlag(PropFlags.Burnt))
        {
            AddAttribute(PropFlags.Burnt);
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
        }
        if (!attributes.HasFlag(PropFlags.OnFire) && toastiness > fireTrigger && firePrefab != null)
        {
            GameObject fire = Instantiate(firePrefab);
            fire.transform.parent = gameObject.transform;
            fire.transform.localPosition = Vector3.zero;
            fire.transform.eulerAngles = Vector3.zero;
            fire.transform.localScale = Vector3.one;
            AddAttribute(PropFlags.OnFire);
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, attributes, true));
            FireEndingManager.instance.addFireObject(gameObject);
            //fireEndingManager.addFireObject(key);
        }
    }

    public void DefrostToast()
    {
        if (attributes.HasFlag(PropFlags.Frozen))
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            attributes &= ~PropFlags.Frozen;
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.ThawObject, attributes, true));
        }
    }
}
