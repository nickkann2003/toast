using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fire Attribute", menuName = "Prop/Attribute/OnFire", order = 53)]
public class OnFireAttribute : PropAttributeSO
{
    [SerializeField]
    private StatType toastType;

    [SerializeField]
    private StatConditional OnFireConditional;

    [SerializeField]
    private GameObject firePrefab;

    [SerializeField]
    private PropIntGameEvent fireEvent;

    //[SerializeField]
    //private PropIntGameEvent extinguishEvent;

    [SerializeField]
    private PropAttributeSO frozenAtt;

    public override void OnEquip(NewProp newProp)
    {
        if (newProp.HasAttribute(frozenAtt))
        {
            newProp.RemoveAttribute(frozenAtt);
            newProp.RemoveAttributeWithoutOnRemove(this);
            return;
        }

        float distToFire = OnFireConditional.Target - newProp.Stats.GetStat(toastType).Value;
        if (distToFire < 0)
        {
            distToFire = 0;
        }
        newProp.IncreaseToastiness(distToFire);

        // Instantiate fire
        GameObject fire = Instantiate(firePrefab);
        fire.transform.parent = newProp.transform;
        fire.transform.localPosition = Vector3.zero;
        fire.transform.eulerAngles = Vector3.zero;
        fire.transform.localScale = Vector3.one * newProp.transform.localScale.x;

        newProp.fireObject = fire;

        // Add attribute
        newProp.AddFlag(PropFlags.OnFire);

        // Trigger objectives
        fireEvent?.RaiseEvent(newProp, 1);

        // Add flaming object
        FireEndingManager.instance.addFireObject(newProp.gameObject);
        //FireEndingManager.instance.addFireObject(newProp.gameObject);
    }

    public override void OnRemove(NewProp newProp)
    {
        FireEndingManager.instance.removeFireObject(newProp.gameObject);
        newProp.RemoveFlag(PropFlags.OnFire);

        Destroy(newProp.fireObject);
        newProp.fireObject = null;
    }
}
