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
    private StatsSystem baseSystem;
    public StatsSystem BaseSystem { get { return baseSystem; } }

    [SerializeField]
    private StatType type; // rename later to be just statType

    public StatType Type { get { return type; } }

    [SerializeField]
    private float baseValue = 0f;
    [SerializeField]
    private float rateOfChange = 1f;

    [SerializeField]
    private List<StatModifier> modifiers = new List<StatModifier>();

    [SerializeField]
    private List<StatConditional> conditionals = new List<StatConditional>();

    private bool isDirty = true;

    [SerializeField, ReadOnly, AllowNesting]
    private float value;

    public float Value
    {
        get
        {
            return UpdateValue();
        }
    }

    public float RateOfChange { get { return rateOfChange; } }

    public Stat(float initialValue) => baseValue = initialValue;

    public Stat(float initialValue, StatType statType, StatsSystem statSystem)
    {
        type = statType;
        baseValue = initialValue;
        rateOfChange = statType.DefaultRateOfChange;
    }

    public Stat(StatType statType, StatsSystem statSystem)
    {
        type = statType;
        baseValue = statType.DefaultValue;
        rateOfChange = statType.DefaultRateOfChange;
    }

    public Stat(Stat _stat, StatsSystem statSystem)
    {
        type = _stat.Type;
        baseValue = _stat.baseValue;
        rateOfChange = _stat.RateOfChange;

        if (_stat.conditionals != null)
        {
            for (int i = 0; i < _stat.conditionals.Count; i++)
            {
                conditionals.Add(_stat.conditionals[i]);
            }
        }

        baseSystem = statSystem;
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

    public void AddConditional(StatConditional condition)
    {
        conditionals.Add(condition);
        condition.Evaluate(this, UpdateValue());
    }

    public void RemoveConditional(StatConditional condition)
    {
        conditionals.Remove(condition);
    }

    //public void RemoveAllModifiers()
    //{
    //    modifiers.Clear();
    //}

    public float UpdateValue()
    {
        if (isDirty)
        {
            value = CalculateValue();

            isDirty = false;
        }

        return value;
    }

    private float CalculateValue()
    {
        float finalValue = baseValue;
        float sumPercentAdditive = 1f;
        float totalPercentMultiplicative = 1f;
        float addOn = 0f;

        if (modifiers.Count > 0)
        {
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
        }

        finalValue = finalValue * sumPercentAdditive * totalPercentMultiplicative + addOn;

        CheckConditionals(finalValue);

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

    public void CheckConditionals(float statValue)
    {
        if (conditionals.Count == 0) { return; }

        for (int i = 0; i < conditionals.Count; i++)
        {
            conditionals[i].Evaluate(this, statValue);
        }
    }
}
