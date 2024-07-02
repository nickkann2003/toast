using UnityEngine;

[CreateAssetMenu(fileName = "New InHand Attribute", menuName = "Prop/Attribute/InHand", order = 53)]
public class InHandAttribute : PropAttributeSO
{
    [SerializeField]
    private bool inMainHand = false;

    public bool InMainHand { get { return inMainHand; } }

    public override void OnEquip(NewProp newProp)
    {
        if (!newProp.GetComponent<Rigidbody>())
        {
            return;
        }
        if (inMainHand)
        {
            newProp.GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            newProp.RemoveRigidbody();
        }
    }

    public override void OnRemove(NewProp newProp)
    {
        if (inMainHand && newProp.GetComponent<Rigidbody>())
        {
            newProp.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            newProp.CreateAndUpdateRigidbody();
        }
    }
}
