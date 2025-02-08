using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Butter Attribute", menuName = "Prop/Attribute/Butter", order = 53)]
public class ButterAttribute : PropAttributeSO
{
    [SerializeField]
    private Stat[] stats;

    [SerializeField]
    protected StatConditionalContainer[] statConditionalContainer;

    public override void OnEquip(NewProp newProp)
    {

        if (stats.Length > 0)
        {
            for (int i = 0; i < stats.Length; i++)
            {
                Stat statToAdd = new Stat(stats[i], newProp.Stats);
                newProp.Stats.AddStat(statToAdd);
            }
        }
        if (statConditionalContainer.Length > 0)
        {
            for (int i = 0; i < statConditionalContainer.Length; i++)
            {
                newProp.Stats.AddConditional(statConditionalContainer[i].Type, statConditionalContainer[i].StatConditional);
            }
        }
    }

    public override void OnRemove(NewProp newProp)
    {
        if (statConditionalContainer.Length > 0)
        {
            for (int i = 0; i < statConditionalContainer.Length; i++)
            {
                newProp.Stats.RemoveConditional(statConditionalContainer[i].Type, statConditionalContainer[i].StatConditional);
            }
        }
    }
}
