using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : NewProp
{
    // ------------------------------- Variables -------------------------------
    [Header("References")]
    [SerializeField]
    public NewHand hand;
    
    private GrabFromContainer grabStrategy;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent useEvent;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Called when this item is used
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
                bool pickedUp = Raycast.Instance.PickupRaycast(hand);
                if(pickedUp) useEvent.RaiseEvent(this, 1);
            }
        }
    }
}
