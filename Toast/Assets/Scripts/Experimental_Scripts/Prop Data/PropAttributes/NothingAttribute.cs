using UnityEngine;

[CreateAssetMenu(fileName = "New Size Attribute", menuName = "Prop/Attribute/Nothing", order = 53)]
public class NothingAttribute : PropAttributeSO
{
    public override void OnEquip(NewProp newProp)
    {
        return;
    }

    public override void OnRemove(NewProp newProp)
    {
        return;
    }
}
