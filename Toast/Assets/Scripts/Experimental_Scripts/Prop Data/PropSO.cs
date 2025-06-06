using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prop Object", menuName = "Prop/Object", order = 53)]
public class PropSO : ScriptableObject
{
    // contexts (inHand, etc.)

    // attributes (onFire, frozen, giant, bread)
    [SerializeField]
    private PropAttributeSO[] attributes;

    [SerializeField]
    private Stat[] stats;

    [SerializeField]
    private bool useBaseStats = true;

    [SerializeField, EnableIf("useBaseStats")]
    private BaseStats baseStats;

    [SerializeField]
    private UseEffectSO[] useEffects;

    [SerializeField, BoxGroup("Configs")]
    private IceConfig iceConfig;

    [SerializeField, BoxGroup("Configs")]
    private PD_Rigidbody rigidbody;

    [SerializeField, BoxGroup("Prop Description")]
    private int propID;

    [SerializeField, BoxGroup("Prop Description")]
    private string displayName;

    [SerializeField, ResizableTextArea, BoxGroup("Prop Description")]
    private string description;
    
    public int PropID { get => propID; }

    public string DisplayName { get => displayName; }
    public string Description { get => description; }

    public void PopulateProp(NewProp newProp)
    {
        if (stats != null)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                Stat statToAdd = new Stat(stats[i], newProp.Stats);
                newProp.Stats.AddStat(statToAdd);
            }
        }

        if (useBaseStats && baseStats != null)
        {
            Stat[] stats = baseStats.Stats;
            for (int i  = 0; i < stats.Length; i++)
            {
                Stat statToAdd = new Stat(stats[i], newProp.Stats);
                newProp.Stats.AddStat(statToAdd);
            }
        }

        if (attributes != null)
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                PropAttributeSO attributeToAdd = attributes[i];
                newProp.AddAttribute(attributeToAdd);
            }
        }

        if (useEffects != null)
        {
            for (int i = 0; i < useEffects.Length; i++)
            {
                UseEffectSO useEffectToAdd = useEffects[i];
                newProp.AddUseEffect(useEffectToAdd);
            }
        }

        if (iceConfig != null)
        {
            newProp.iceConfig = iceConfig;
        }

        if (rigidbody != null)
        {
            newProp.PD_Rb = rigidbody;
        }
    }
}

