using UnityEngine;

[CreateAssetMenu(fileName = "New Fire Attribute", menuName = "Prop/Attribute/OnFire", order = 53)]
public class OnFireAttribute : PropAttributeSO
{
    [SerializeField]
    private StatType toastType;

    private StatModifier modifier = new StatModifier(StatModifierTypes.AddOn, 2.5f);

    public override void OnEquip(NewProp newProp)
    {
        newProp.Stats.AddModifier(toastType, modifier);
        newProp.IncreaseToastiness(0);
        //FireEndingManager.instance.addFireObject(newProp.gameObject);
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.Stats.RemoveModifier(toastType, modifier);
        FireEndingManager.instance.removeFireObject(newProp.gameObject);
        newProp.RemoveFlag(PropFlags.OnFire);

        Destroy(newProp.fireObject);
        newProp.fireObject = null;
    }
}
