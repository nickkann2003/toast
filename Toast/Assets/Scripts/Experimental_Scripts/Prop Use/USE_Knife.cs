using UnityEngine;

[CreateAssetMenu(fileName = "New Knife Effect", menuName = "Prop/Use Effect/Knife", order = 53)]
public class USE_Knife : UseEffectSO
{
    [SerializeField]
    private InHandAttribute inHandAttribute;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent useEvent;

    public override void Use(NewProp newProp)
    {
        NewHand newHand = newProp.GetComponentInChildren<NewHand>();
        if (newHand == null) { return; }

        if (newProp.HasAttribute(inHandAttribute))
        {
            if (newHand.CheckObject())
            {
                newHand.UseInHand();
            }
            else
            {
                bool pickedUp = Raycast.Instance.PickupRaycast(newHand);
                if (pickedUp) useEvent.RaiseEvent(newProp, 1);
            }
        }
    }
}
