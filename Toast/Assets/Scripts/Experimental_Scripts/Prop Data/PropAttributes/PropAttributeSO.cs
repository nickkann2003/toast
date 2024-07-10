using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PropAttributeSO : ScriptableObject
{
    public abstract void OnEquip(NewProp newProp);
    public abstract void OnRemove(NewProp newProp);
}
