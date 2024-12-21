using NaughtyAttributes;
using UnityEngine;
[CreateAssetMenu(fileName = "New Place Effect", menuName = "Prop/Use Effect/Place", order = 53)]
public class USE_Jam : UseEffectSO
{
    //[Header("Number of Uses")]
    //[SerializeField]
    //private int total = -1;
    //[SerializeField]
    //private int remaining = -1;

    [SerializeField]
    private PropAttributeSO lidAtt;

    //[Header("Prefabs")]
    //[SerializeField]
    //private GameObject objPrefab;
    //[SerializeField]
    //private Material mat;

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
    [SerializeField, EnableIf("invokeEvents")]
    private PropIntGameEvent jamEvent;

    [SerializeField]
    private SimpleAudioEvent jamSplashEvent;

    //public override void OnEquip(NewProp prop)
    //{
        
    //}

    public override bool TryUse(NewProp newProp)
    {
        Jam jam = newProp.GetComponent<Jam>();
        if (jam == null)
        {
            return false;
        }
        JamConfig config = jam.Config;

        if (jam.IsCapped)
        {
            jam.UncapJam();
            return true;
        }
        // get placement
        // CHANGE LATER
        RaycastHit hit = Raycast.Instance.RaycastHelper(~(1 << 10) & ~(1 << 3));

        if (hit.collider.gameObject != null)
        {
            NewProp prop = hit.collider.gameObject.GetComponent<NewProp>();
            if (prop != null)
            {
                if (prop.HasAttribute(lidAtt))
                {
                    jam.CapJam(prop.gameObject);
                    return true;
                }
            }

            GameObject obj = GameObject.Instantiate(config.SplatPrefab);
            obj.transform.position = hit.point + hit.normal * .01f;
            obj.transform.up = hit.normal;

            obj.transform.parent = hit.collider.gameObject.transform;
            obj.transform.GetChild(0).Rotate(new Vector3(0, 0, Random.Range(-30, 30) * 2), Space.Self);
            obj.GetComponentInChildren<Renderer>().material.color = config.Material.color;
            jamEvent.RaiseEvent(newProp, 1);
            useEvent.RaiseEvent(newProp, 1);

            AudioManager.instance.PlayAudioEvent(jamSplashEvent);

            return true;
        }

        return false;
    }
}
