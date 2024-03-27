using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : NewProp
{
    public NewHand hand;
    GrabFromContainer grabStrategy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Use()
    {
        if (attributes.HasFlag(PropFlags.InHand))
        {
            if (hand.CheckObject())
            {
                hand.UseInHand();
            }
            else
            {
                Raycast.Instance.PickupRaycast(hand);
                ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.UseObject, attributes, true));
            }
        }
    }
}
