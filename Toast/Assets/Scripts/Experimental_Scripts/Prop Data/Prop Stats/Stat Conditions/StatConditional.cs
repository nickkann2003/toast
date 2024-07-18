using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Conditional", menuName = "Prop/Stats/Conditional", order = 53)]
public class StatConditional : ScriptableObject
{
    [Dropdown("GetOperator"), Label("Operator")]
    public int opNum = 0;

    [SerializeField]
    private float targetValue;
    public float Target { get { return targetValue; } }

    [SerializeField]
    private PropAttributeSO attToGive;

    [SerializeField]
    private bool removeOnCompletion = true;

    [SerializeField]
    private SimpleAudioEvent audioEvent;

    private DropdownList<int> GetOperator()
    {
        return new DropdownList<int>()
        {
            { "<=",   -1 },
            { "==",   0 },
            { ">=",   1 }
        };
    }

    public void Evaluate(Stat stat, float statValue)
    {
        if (statValue == targetValue || (statValue - targetValue) * opNum > 0)
        {
            if (!stat.BaseSystem.BaseProp.HasAttribute(attToGive))
            {
                stat.BaseSystem.BaseProp.AddAttribute(attToGive);
            }

            if(audioEvent != null)
            {
                stat.BaseSystem.BaseProp.PlayAudioEvent(audioEvent);
            }

            if (removeOnCompletion)
            {
                stat.RemoveConditional(this);
            }
        }
    }
}

[Serializable]
public class StatConditionalContainer
{
    [SerializeField]
    private StatType statType;
    [SerializeField]
    private StatConditional statConditional;

    public StatType Type => statType;
    public StatConditional StatConditional => statConditional;
}
