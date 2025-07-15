using Steamworks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combustible Attribute", menuName = "Prop/Attribute/Combustible", order = 53)]
public class CombustibleAttribute : PropAttributeSO
{
    [SerializeField]
    private StatConditionalContainer statConditionalContainer;


    public override void OnEquip(NewProp newProp)
    {
        newProp.Stats.AddConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.Stats.RemoveConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
    }
}
