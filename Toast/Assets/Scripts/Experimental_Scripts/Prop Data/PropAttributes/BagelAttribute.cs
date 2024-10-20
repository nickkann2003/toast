using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Bagel Attribute", menuName = "Prop/Attribute/Bagel", order = 53)]
public class BagelAttribute : PropAttributeSO
{
    public override void OnEquip(NewProp newProp)
    {
        // Change prop model to bagel
    }

    public override void OnRemove(NewProp newProp)
    {

    }
}
