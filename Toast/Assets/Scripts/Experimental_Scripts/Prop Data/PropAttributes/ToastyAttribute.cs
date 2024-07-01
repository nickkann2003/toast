using UnityEngine;

[CreateAssetMenu(fileName = "New Toasty Attribute", menuName = "Prop/Attribute/Toasty", order = 53)]
public class ToastyAttribute : PropAttributeSO
{
    [SerializeField]
    private StatConditionalContainer statConditionalContainer;

    [SerializeField]
    private PropIntGameEvent propIntGameEvent;

    [SerializeField]
    private PropFlags flag;

    public override void OnEquip(NewProp newProp)
    {
        newProp.Stats.AddConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
        newProp.AddFlag(flag);
        propIntGameEvent?.RaiseEvent(newProp, 1);
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.Stats.RemoveConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
        newProp.RemoveFlag(flag);
    }
}
