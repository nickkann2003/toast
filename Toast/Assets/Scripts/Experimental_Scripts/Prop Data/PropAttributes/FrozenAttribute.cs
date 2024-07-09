using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Frozen Attribute", menuName = "Prop/Attribute/Frozen", order = 53)]
public class FrozenAttribute : PropAttributeSO
{
    [SerializeField]
    private StatType freezeType;

    [SerializeField]
    private StatConditionalContainer FreezeConditional;

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
            newProp.StartCoroutine(RemoveNextFrame(newProp));
            //newProp.RemoveAttribute(onFireAtt);
            
            //newProp.RemoveAttributeWithoutOnRemove(this);
            ////newProp.Stats.IncrementStat(freezeType, -newProp.Stats.GetStat(freezeType).Value);

            //newProp.Stats.AddConditional(FreezeConditional.Type, FreezeConditional.StatConditional);
            return;
        }
        // check to see if it is on fire
        // if it is on fire remove fire and this

        //float distToFreeze = FreezeConditional.Target - newProp.Stats.GetStat(freezeType).Value;
        //if (distToFreeze < 0)
        //{
        //    distToFreeze = 0;
        //}
        //newProp.Stats.IncrementStat(freezeType, distToFreeze);

        ////float frozenNum = -newProp.Stats.GetStat(freezeType).Value;
        //Stat freezeStat = newProp.Stats.GetStat(freezeType);

        //newProp.Stats.IncrementStat(freezeType, freezeStat.Value);
        //freezeStat.UpdateValue();


        // CALL FREEZE EVENT???
        freezeEvent?.RaiseEvent(newProp, 1);

        GameObject obj = newProp.StaticMesh;

        // add the ice prefab to it
        GameObject ice = Instantiate(icePrefab);
        //ice.transform.position = obj.transform.position;
        ice.GetComponent<MeshFilter>().sharedMesh = obj.GetComponentInChildren<MeshFilter>().sharedMesh;
        ice.transform.parent = obj.transform;
        ice.transform.localPosition = newProp.iceConfig.Offset;
        ice.transform.localEulerAngles = Vector3.zero;
        ice.transform.localScale = newProp.iceConfig.Scale;
        //ice.transform.localScale = new Vector3(1.1f, 1.1f, 1.4f);

        newProp.iceObject = ice;

        newProp.AddFlag(PropFlags.Frozen);
    }

    IEnumerator RemoveNextFrame(NewProp newProp)
    {
        newProp.RemoveAttribute(onFireAtt);
        yield return new WaitForSeconds(0);
        newProp.RemoveAttributeWithoutOnRemove(this);
        newProp.Stats.IncrementStat(freezeType, -newProp.Stats.GetStat(freezeType).Value);

        newProp.Stats.AddConditional(FreezeConditional.Type, FreezeConditional.StatConditional);
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.Stats.IncrementStat(freezeType, -newProp.Stats.GetStat(freezeType).Value);

        newProp.Stats.AddConditional(FreezeConditional.Type, FreezeConditional.StatConditional);

        thawEvent?.RaiseEvent(newProp, 1);

        newProp.RemoveFlag(PropFlags.Frozen);

        Destroy(newProp.iceObject);
        newProp.iceObject = null;
    }
}
