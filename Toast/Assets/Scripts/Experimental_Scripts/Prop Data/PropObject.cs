using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prop Object", menuName = "Prop/Object", order = 53)]
public class PropObject : ScriptableObject
{
    // contexts (inHand, etc.)

    // attributes (onFire, frozen, giant, bread)
    [SerializeField]
    private PropAttributeObject[] attributes;

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

        //if (attributes != null)
        //{
            
        //}
    }
}

