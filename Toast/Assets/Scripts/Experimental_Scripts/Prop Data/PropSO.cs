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

    public void PopulateProp(NewProp newProp)
    {
        if (stats != null)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                Stat statToAdd = new Stat(stats[i]);
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
    }
}

