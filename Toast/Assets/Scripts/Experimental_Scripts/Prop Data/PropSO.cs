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
    private BaseStats baseStats;

    //[SerializeField]
    //private Stat[] stats;

    [SerializeField, Foldout("Configs")]
    private IceConfig iceConfig;

    [SerializeField, Foldout("Configs")]
    private PD_Rigidbody rigidbody;

    public void PopulateProp(NewProp newProp)
    {
        //if (stats != null)
        //{
        //    for (int i = 0; i < stats.Length; i++)
        //    {
        //        Stat statToAdd = new Stat(stats[i], newProp.Stats);
        //        newProp.Stats.AddStat(statToAdd);
        //    }
        //}

        if (baseStats != null)
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
                newProp.attributesList.Add(attributeToAdd);
                attributeToAdd.OnEquip(newProp);
            }
        }

        //if (useEffects != null)
        //{
        //    for (int i = 0; i < useEffects.Length; i++)
        //    {
        //        UseEffectSO useEffectToAdd = useEffects[i];
        //        newProp.useEffects.Add(useEffectToAdd);
        //    }
        //}
    }
}

