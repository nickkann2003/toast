using UnityEngine;

public class USE_Pickup : UseEffectSO
{
    public override bool TryUse(NewProp newProp)
    {
        //NewHand hand = newProp.GetComponentInChildren<NewHand>().
        return true;
    }
}
