using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class NewProp : MonoBehaviour
{
    [SerializeField]
    private Material baseMat;

    public PropFlags attributes;

    public float toastiness;
    public float frozenness;

    protected IUseStrategy _useStrategy;

    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _useStrategy = this.gameObject.GetComponent<IUseStrategy>();
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
}
