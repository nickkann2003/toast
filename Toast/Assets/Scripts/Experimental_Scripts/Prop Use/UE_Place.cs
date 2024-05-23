using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Place Effect", menuName = "Prop/Use Effect/Place", order = 53)]
public class UE_Place : UseEffect
{
    // bool
    public bool invokeEvents;

    // events
    [EnableIf("invokeEvents")]
    public GameEvent gameEvent;

    // audio
    public AudioSource audioSource;
    public AudioEvent eatAudioEvent;

    // obj to place
    public GameObject objPrefab;

    public override void Use(NewProp prop)
    {
        // -- check for condition --
        //// If jam, uncap it before it can be used
        //if (isJam)
        //{
        //    if (jam.IsCapped)
        //    {
        //        jam.UncapJam();
        //        return;
        //    }
        //}

        //if (remaining == 0 || objPrefab == null || !transform.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
        //{
        //    return;
        //}

        // -- check for placement --

        // -- if there is a valid placement, then instantiate the objPrefab and place it --
    }
}
