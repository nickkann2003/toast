using Steamworks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Toasty Attribute", menuName = "Prop/Attribute/Toasty", order = 53)]
public class ToastyAttribute : PropAttributeSO
{
    [SerializeField]
    private StatConditionalContainer statConditionalContainer;

    [SerializeField]
    private PropIntGameEvent propIntGameEvent;

    [SerializeField]
    private PropFlags flag;

    public override void OnEquip(NewProp newProp)
    {
        newProp.Stats.AddConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
        newProp.AddFlag(flag);
        propIntGameEvent?.RaiseEvent(newProp, 1);

        // Local stats integration
        SaveHandler.instance.StatsHandler.ToastMade += 1;

        // Steam stats integration
        int toastMade = 0;
        SteamUserStats.GetStat("TOAST_MADE", out toastMade);
        toastMade++;
        SteamUserStats.SetStat("TOAST_MADE", toastMade);
        SteamUserStats.StoreStats();
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.Stats.RemoveConditional(statConditionalContainer.Type, statConditionalContainer.StatConditional);
        newProp.RemoveFlag(flag);
    }
}
