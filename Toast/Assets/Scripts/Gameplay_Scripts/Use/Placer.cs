using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour, IUseStrategy
{
    [SerializeField]
    int total = -1;
    [SerializeField]
    int remaining = -1;

    [SerializeField]
    GameObject objPrefab;
    [SerializeField]
    Material mat;

    GameObject parentPlacementObj;
    Vector3 placementLocation;
    Vector3 placementRotation;

    [SerializeField]
    private bool isJam = false;
    [EnableIf("isJam")]
    [Header("Jam References")]
    [SerializeField]
    private Jam jam;

    public void Use()
    {
        // If jam, uncap it before it can be used
        if (isJam)
        {
            if (jam.IsCapped)
            {
                jam.UncapJam();
                return;
            }
        }

        if (remaining == 0 || objPrefab == null || !transform.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
        {
            return;
        }

        SetPlacement(Raycast.Instance.RaycastHelper(~(1 << 10) & ~(1 << 3)));

        if (parentPlacementObj != null)
        {
            GameObject obj = GameObject.Instantiate(objPrefab);
            obj.transform.position = placementLocation;
            obj.transform.up = placementRotation;
            
            obj.transform.parent = parentPlacementObj.transform;
            obj.transform.GetChild(0).Rotate(new Vector3(0, 0, Random.Range(-30, 30)*2), Space.Self);
            obj.GetComponentInChildren<Renderer>().material.color = mat.color;
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.UseObject, gameObject.GetComponent<NewProp>().attributes, true));
        }
    }

    public void SetPlacement(RaycastHit hit)
    {
        if (hit.collider != null)
        {
            parentPlacementObj = hit.collider.gameObject;
            placementLocation = hit.point + hit.normal * .01f;
            placementRotation = hit.normal;
        }
    }
}
