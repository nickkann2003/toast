using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Size Attribute", menuName = "Prop/Attribute/OnFire", order = 53)]
public class OnFireAttribute : PropAttributeObject
{
    [SerializeField]
    private StatType toastType;

    private StatModifier modifier = new StatModifier(StatModifierTypes.Flat, 100);

    public override void OnEquip(NewProp newProp)
    {
        newProp.Stats.AddModifier(toastType, modifier);
        newProp.IncreaseToastiness(0);
        //FireEndingManager.instance.addFireObject(newProp.gameObject);
    }

    public override void OnRemove(NewProp newProp)
    {
        FireEndingManager.instance.removeFireObject(newProp.gameObject);
    }
}
