using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Bagel Attribute", menuName = "Prop/Attribute/Bagel", order = 53)]
public class BagelAttribute : PropAttributeSO
{
    [SerializeField]
    GameObject bagelPrefab;

    [SerializeField]
    Mesh bagelMesh;

    [SerializeField]
    Material bagelMat;

    [SerializeField]
    Vector3 bagelColliderSize = new Vector3(0.48f, 0.12f, -0.51f);

  

    public override void OnEquip(NewProp newProp)
    {
        // Change prop model to bagel
        newProp.GetComponentInChildren<MeshFilter>().mesh = bagelMesh;
        newProp.GetComponentInChildren<MeshRenderer>().material= bagelMat;
        newProp.transform.GetChild(0).transform.localScale = Vector3.one;
        newProp.GetComponent<BoxCollider>().size = bagelColliderSize;
        newProp.transform.rotation = Quaternion.Euler(0, 0, -90);
        newProp.IncreaseToastiness(0);
       
    }

    public override void OnRemove(NewProp newProp)
    {

    }
}
