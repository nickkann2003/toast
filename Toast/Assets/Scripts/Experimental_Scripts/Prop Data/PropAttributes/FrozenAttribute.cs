using UnityEngine;

[CreateAssetMenu(fileName = "New Frozen Attribute", menuName = "Prop/Attribute/Frozen", order = 53)]
public class FrozenAttribute : PropAttributeSO
{
    [SerializeField]
    private StatType freezeType;

    [SerializeField]
    private StatConditional FreezeConditional;

    [SerializeField]
    private GameObject icePrefab;

    [SerializeField]
    private PropIntGameEvent freezeEvent;

    [SerializeField]
    private PropIntGameEvent thawEvent;

    [SerializeField]
    private PropAttributeSO onFireAtt;

    //NEEDS WORK
    public override void OnEquip(NewProp newProp)
    {
        if (newProp.HasAttribute(onFireAtt))
        {
            newProp.RemoveAttribute(onFireAtt);
            newProp.RemoveAttributeWithoutOnRemove(this);
            return;
        }
        // check to see if it is on fire
        // if it is on fire remove fire and this

        float distToFreeze = FreezeConditional.Target - newProp.Stats.GetStat(freezeType).Value;
        if (distToFreeze < 0)
        {
            distToFreeze = 0;
        }
        newProp.Stats.IncrementStat(freezeType, distToFreeze);

        // CALL FREEZE EVENT???
        freezeEvent?.RaiseEvent(newProp, 1);

        GameObject obj = newProp.StaticMesh;

        // add the ice prefab to it
        GameObject ice = Instantiate(icePrefab);
        //ice.transform.position = obj.transform.position;
        ice.GetComponent<MeshFilter>().sharedMesh = obj.GetComponentInChildren<MeshFilter>().sharedMesh;
        ice.transform.parent = obj.transform;
        ice.transform.localPosition = newProp.IceConfig.Offset;
        ice.transform.localEulerAngles = Vector3.zero;
        ice.transform.localScale = newProp.IceConfig.Scale;
        //ice.transform.localScale = new Vector3(1.1f, 1.1f, 1.4f);

        newProp.iceObject = ice;

        newProp.AddFlag(PropFlags.Frozen);
    }

    public override void OnRemove(NewProp newProp)
    {
        thawEvent?.RaiseEvent(newProp, 1);

        newProp.RemoveFlag(PropFlags.Frozen);

        Destroy(newProp.iceObject);
        newProp.iceObject = null;
    }
}
