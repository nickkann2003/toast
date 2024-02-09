using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewProp : MonoBehaviour
{

    // in order to check to see if the prop contains a flag use HasFlag
    // in order to check to see if the prop doesn't contain a flag use Equals(PropFlags.None)
    [Flags]
    public enum PropFlags
    {
        None = 0,
        Toast = 1 << 0,
        Jammed = 1 << 1,
        Metal = 1 << 2,
        Giant = 1 << 3,
        ImmuneToFreeze = 1 << 4,
        ImmuneToToast = 1 << 5,
        InHand
    }
    public PropFlags attributes;

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

    public virtual void Use(GameObject gameObject)
    {
        _useStrategy?.Use(gameObject);
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
