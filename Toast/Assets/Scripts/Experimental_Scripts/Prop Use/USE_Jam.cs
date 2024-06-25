using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Place Effect", menuName = "Prop/Use Effect/Place", order = 53)]
public class USE_Jam : UseEffectSO
{
    //[Header("Number of Uses")]
    //[SerializeField]
    //private int total = -1;
    //[SerializeField]
    //private int remaining = -1;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject objPrefab;
    [SerializeField]
    private Material mat;

    // audio
    //public AudioSource audioSource;
    //public AudioEvent eatAudioEvent;

    //TargetSO

    // events
    [Header("Event References")]
    [SerializeField]
    private bool invokeEvents;
    [SerializeField, EnableIf("invokeEvents")]
    private PropIntGameEvent useEvent;

    //public override void OnEquip(NewProp prop)
    //{
        
    //}

    public override void Use(NewProp newProp)
    {
        // get placement
        // CHANGE LATER
        RaycastHit hit = Raycast.Instance.RaycastHelper(~(1 << 10) & ~(1 << 3));

        if (hit.collider.gameObject != null)
        {
            GameObject obj = GameObject.Instantiate(objPrefab);
            obj.transform.position = hit.point + hit.normal * .01f;
            obj.transform.up = hit.normal;

            obj.transform.parent = hit.collider.gameObject.transform;
            obj.transform.GetChild(0).Rotate(new Vector3(0, 0, Random.Range(-30, 30) * 2), Space.Self);
            obj.GetComponentInChildren<Renderer>().material.color = mat.color;
            useEvent.RaiseEvent(newProp, 1);
        }
    }
}
