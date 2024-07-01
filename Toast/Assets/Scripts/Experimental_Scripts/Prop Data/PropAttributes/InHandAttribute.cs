using UnityEngine;

[CreateAssetMenu(fileName = "New InHand Attribute", menuName = "Prop/Attribute/InHand", order = 53)]
public class InHandAttribute : PropAttributeSO
{
    public override void OnEquip(NewProp newProp)
    {
        newProp.RemoveRigidbody();
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.CreateAndUpdateRigidbody();
    }
}
