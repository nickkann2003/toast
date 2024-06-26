using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InHand Attribute", menuName = "Prop/Attribute/InHand", order = 53)]
public class InHandAttribute : PropAttributeSO
{
    public override void OnEquip(NewProp newProp)
    {
        if (newProp.GetComponent<Rigidbody>() != null)
        {
            Destroy(newProp.GetComponent<Rigidbody>());
        }
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.CreateAndUpdateRigidbody();
    }
}
