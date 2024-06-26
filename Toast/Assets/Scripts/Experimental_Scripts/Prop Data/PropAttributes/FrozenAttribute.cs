using UnityEngine;

[CreateAssetMenu(fileName = "New Frozen Attribute", menuName = "Prop/Attribute/Frozen", order = 53)]
public class FrozenAttribute : PropAttributeSO
{
    [SerializeField]
    private GameObject icePrefab;

    //[SerializeField]
    //private PropIntGameEvent freezeEvent;

    //NEEDS WORK
    public override void OnEquip(NewProp newProp)
    {
        // check to see if it is on fire
        // if it is on fire remove fire and this

        GameObject obj = newProp.gameObject;

        // add the ice prefab to it
        GameObject ice = Instantiate(icePrefab);
        ice.transform.position = obj.transform.position;
        ice.GetComponent<MeshFilter>().sharedMesh = obj.GetComponentInChildren<MeshFilter>().sharedMesh;
        ice.transform.parent = obj.transform;
        ice.transform.localEulerAngles = Vector3.zero;
        ice.transform.localScale = new Vector3(1.1f, 1.1f, 1.4f);

        newProp.iceObject = ice;

        newProp.AddFlag(PropFlags.Frozen);
    }

    public override void OnRemove(NewProp newProp)
    {
        newProp.RemoveFlag(PropFlags.Frozen);

        Destroy(newProp.iceObject);
        newProp.iceObject = null;
    }
}
