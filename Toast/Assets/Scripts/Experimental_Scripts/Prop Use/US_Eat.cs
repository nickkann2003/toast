using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Eat Effect", menuName = "Prop/Use Effect/Eat", order = 53)]
public class US_Eat : UseEffect
{
    // bool
    public bool invokeEvents;

    // events
    //[EnableIf("invokeEvents")]
    //public GameEvent gameEvent;

    // audio
    public AudioSource audioSource;
    public AudioEvent eatAudioEvent;

    // particles

    public override void Use(NewProp prop)
    {
        // play sound
        eatAudioEvent.Play(audioSource);

        // play particles

        // decrease bitesRemaining

        // if bitesRemaining <= 0, then Eat

    }
}
