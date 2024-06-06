using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

public class Stat
{
    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    private float baseValue = 0f;
    private bool isDirty = true;

    private float value;
    private float rateOfChange = 1;

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

    public Stat(float initialValue) => baseValue = initialValue;
    public Stat(StatType statType) => baseValue = statType.DefaultValue;

    public void UpdateBaseValue(float amount)
    {
        isDirty = true;

        value += amount * rateOfChange;
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
                    totalPercentMultiplicative *= 1 + modifier.Value;
                    break;
            }
        }

        finalValue = finalValue * sumPercentAdditive * totalPercentMultiplicative;

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
}
