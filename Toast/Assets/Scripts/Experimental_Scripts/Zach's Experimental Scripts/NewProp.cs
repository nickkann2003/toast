using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewProp : MonoBehaviour
{
    [SerializeField]
    private Material baseMat;

    public PropFlags attributes;

    private float toastiness;

    protected IUseStrategy _useStrategy;

    // Start is called before the first frame update
    void Start()
    {
        _useStrategy = this.gameObject.GetComponent<IUseStrategy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void Use()
    {
        _useStrategy?.Use();
    }

    public void AddAttribute(PropFlags flagToAdd)
    {
        attributes |= flagToAdd;
    }

    public void RemoveAttribute(PropFlags flagToRemove)
    {
        attributes &= ~flagToRemove;
    }
}
