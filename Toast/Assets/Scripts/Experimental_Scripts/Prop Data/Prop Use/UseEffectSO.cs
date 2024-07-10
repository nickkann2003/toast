using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseEffectSO : ScriptableObject
{
    public virtual void OnEquip(NewProp prop)
    {
        return;
    }

    public abstract bool TryUse(NewProp prop);

    public virtual void OnRemove(NewProp prop)
    {
        return;
    }
}
