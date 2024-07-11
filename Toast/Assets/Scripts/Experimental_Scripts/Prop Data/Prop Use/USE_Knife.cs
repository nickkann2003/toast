using UnityEngine;

[CreateAssetMenu(fileName = "New Knife Effect", menuName = "Prop/Use Effect/Knife", order = 53)]
public class USE_Knife : UseEffectSO
{
    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent useEvent;

    public override bool TryUse(NewProp newProp)
    {
        NewHand newHand = newProp.GetComponentInChildren<NewHand>();
        if (newHand == null) { return false; }
        if (newProp.HasAttribute(StatAttManager.instance.inHandAtt))
        {
            if (newHand.CheckObject())
            {
                // If object is a spread, set it on knife
                if(newHand.CheckObject().TryGetComponent(out Spread spread))
                {
                    spread.IsOnKnife = true;
                }

                if (!newHand.TryUseInHand())
                {

                    // Object is a spread, set no longer on knife
                    if (newHand.CheckObject().TryGetComponent(out spread))
                    {
                        spread.IsOnKnife = false;
                    }

                    // CHANGE LATER
                    newHand.Drop().transform.position = StationManager.instance.playerLocation.ObjectOffset; // CHANGE LATER
                }

                return true;
            }
            else
            {
                bool pickedUp = Raycast.Instance.PickupRaycast(newHand);
                if (pickedUp)
                {
                    useEvent.RaiseEvent(newProp, 1);
                    return true;
                }
            }
        }

        return false;
    }
}
