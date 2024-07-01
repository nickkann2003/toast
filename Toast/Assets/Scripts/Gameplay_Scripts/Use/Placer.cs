using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour, IUseStrategy
{
    // ------------------------------- Variables -------------------------------
    [Header("Number of Uses")]
    [SerializeField]
    private int total = -1;
    [SerializeField]
    private int remaining = -1;

    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> objPrefabs = new List<GameObject>();
    [SerializeField]
    private Material mat;

    private GameObject parentPlacementObj;
    private Vector3 placementLocation;
    private Vector3 placementRotation;

    [Header("Jam Specific Variables")]
    [SerializeField]
    private bool isJam = false;
    [EnableIf("isJam")]
    [BoxGroup("Jam")]
    [SerializeField]
    private Jam jam;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent useEvent;

    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Use function for this item
    /// </summary>
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

        if (remaining == 0 || objPrefabs.Count <= 0 || !transform.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
        {
            return;
        }

        SetPlacement(Raycast.Instance.RaycastHelper(~(1 << 10) & ~(1 << 3)));

        if (parentPlacementObj != null)
        {
            float rand = Random.value;
            float segmenter = 1.01f/objPrefabs.Count;
            int result = (int) (rand / segmenter);
            GameObject obj = GameObject.Instantiate(objPrefabs[result]);
            obj.transform.position = placementLocation;
            obj.transform.up = placementRotation;

            float randVal = 0.4f;
            obj.transform.localScale = new Vector3(0.8f + (Random.value * randVal - randVal/2f), 0.8f + (Random.value * randVal - randVal / 2f), 0.8f + (Random.value * randVal - randVal / 2f));
            obj.transform.parent = parentPlacementObj.transform;
            obj.transform.GetChild(0).Rotate(new Vector3(0, 0, Random.Range(-30, 30)*2), Space.Self);
            obj.GetComponentInChildren<Renderer>().material.color = mat.color;
            
            Color c = obj.GetComponentInChildren<Renderer>().material.color;
            float colorRandVal = 0.1f;
            c.r *= (Random.value * colorRandVal - colorRandVal / 2f) + 1.0f;
            c.g *= (Random.value * colorRandVal - colorRandVal / 2f) + 1.0f;
            c.b *= (Random.value * colorRandVal - colorRandVal / 2f) + 1.0f;
            obj.GetComponentInChildren<Renderer>().material.color = c;
            useEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), 1);
        }
    }

    /// <summary>
    /// Places object
    /// </summary>
    /// <param name="hit">Racyast hit of where to place</param>
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
