using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [SerializeField]
    private StatModifierTypes modifierType;
    [SerializeField]
    private float value;

    public StatModifier(StatModifierTypes modifierType, float value)
    {
        this.modifierType = modifierType;
        this.value = value;
    }

    public float Value => value;
    public StatModifierTypes ModifierType => modifierType;
}

[Serializable]
public class StatModifierContainer
{
    [SerializeField]
    private StatType statType;
    [SerializeField]
    private StatModifier statModifier;

    public StatType Type => statType;
    public StatModifier StatModifier => statModifier;
}