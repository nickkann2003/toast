using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Size Attribute", menuName = "Prop/Attribute/Size", order = 53)]
public class SizeAttributeObject : PropAttributeObject
{
    public override void OnEquip(NewProp newProp)
    {
        if (statModifierContainer != null && statModifierContainer.Length > 0)
        {
            for (int i = 0; i < statModifierContainer.Length; i++)
            {
                newProp.Stats.AddModifier(statModifierContainer[i].Type, statModifierContainer[i].StatModifier);
                //newProp.AddStatOfType(statModifiers[i].Type);
                //newProp.AddModifier(statModifiers[i].Type, statModifiers[i].StatModifier);
            }
        }
        
        newProp.RecalcSize();
    }

    public override void OnRemove(NewProp newProp)
    {
        if (statModifierContainer != null && statModifierContainer.Length > 0)
        {
            for (int i = 0; i < statModifierContainer.Length; i++)
            {
                newProp.Stats.RemoveModifier(statModifierContainer[i].Type, statModifierContainer[i].StatModifier);
            }
        }
    }
}
