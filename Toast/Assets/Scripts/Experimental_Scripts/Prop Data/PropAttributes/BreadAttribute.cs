using UnityEngine;

[CreateAssetMenu(fileName = "New Bread Attribute", menuName = "Prop/Attribute/Bread", order = 53)]
public class BreadAttribute : PropAttributeSO
{
    [SerializeField]
    private Stat[] stats;

    [SerializeField]
    protected StatConditionalContainer[] statConditionalContainer;

    [SerializeField]
    private UseEffectSO[] useEffects;

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
        if (useEffects.Length > 0)
        {
            for (int i = 0; i < useEffects.Length; i++)
            {
                newProp.useEffects.Add(useEffects[i]);
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