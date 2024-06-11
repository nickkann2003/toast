using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

[Serializable]
public class Stat
{
    [SerializeField]
    private StatType type; // rename later to be just statType

    public StatType Type { get { return type; } }

    [SerializeField]
    private float baseValue = 0f;
    [SerializeField]
    private float rateOfChange = 1f;

    [SerializeField]
    private List<StatModifier> modifiers = new List<StatModifier>();

    private bool isDirty = true;

    [SerializeField, ReadOnly, AllowNesting]
    private float value;

    public float Value
    {
        get
        {
            if (isDirty)
            {
                value = CalculateValue();
                isDirty = false;
            }
            return value;
        }
    }

    public float RateOfChange { get { return rateOfChange; } }

    public Stat(float initialValue) => baseValue = initialValue;
    public Stat(StatType statType)
    {
        baseValue = statType.DefaultValue;
        rateOfChange = statType.DefaultRateOfChange;
    }

    public Stat(Stat _stat)
    {
        type = _stat.Type;
        baseValue = _stat.baseValue;
        rateOfChange = _stat.RateOfChange;
    }

    public void IncreaseValue(float amount)
    {
        isDirty = true;

        baseValue += amount * rateOfChange;
    }

    public void AddModifier(StatModifier modifier)
    {
        // set value to dirty
        isDirty = true;

        // put modifiers in priority order
        int index = modifiers.BinarySearch(modifier, new ByPriority());
        if (index < 0) { index = ~index; }
        modifiers.Insert(index, modifier);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        isDirty = true;
        modifiers.Remove(modifier);
    }

    //public void RemoveAllModifiers()
    //{
    //    modifiers.Clear();
    //}

    private float CalculateValue()
    {
        float finalValue = baseValue;
        float sumPercentAdditive = 1f;
        float totalPercentMultiplicative = 1f;
        float addOn = 0f;

        for (int i = 0; i < modifiers.Count; i++)
        {
            var modifier = modifiers[i];

            switch (modifier.ModifierType)
            {
                case StatModifierTypes.Flat:
                    finalValue += modifier.Value;
                    break;

                case StatModifierTypes.PercentAdditive:
                    sumPercentAdditive += modifier.Value;
                    break;

                case StatModifierTypes.PercentMultiplicative:
                    totalPercentMultiplicative *= modifier.Value;
                    break;

                case StatModifierTypes.AddOn:
                    addOn += modifier.Value;
                    break;
            }
        }

        finalValue = finalValue * sumPercentAdditive * totalPercentMultiplicative + addOn;

        return finalValue;
    }

    // allows us to order stat modifiers (Flat < PercentAdditive < PercentMultiplicative
    private class ByPriority : IComparer<StatModifier>
    {
        public int Compare(StatModifier x, StatModifier y)
        {
            if (x.ModifierType > y.ModifierType) { return 1; }
            if (x.ModifierType < y.ModifierType) { return -1; }
            return 0;
        }
    }

    public override string ToString()
    {
        return $"{type.Name}: {Value}";
    }
}
