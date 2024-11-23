using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Eat Effect", menuName = "Prop/Use Effect/Eat", order = 53)]
public class USE_Eat : UseEffectSO
{
    // ------------------------------- Variables -------------------------------

    [Header("Eating Variables")]
    [SerializeField]
    private StatType biteType;
    [SerializeField]
    private int totalBites;
    [SerializeField]
    private SimpleAudioEvent eatSound;

    //// audio
    //private AudioSource audioSource;
    //private AudioEvent eatAudioEvent;

    // particles

    // events
    [Header("Event References")]
    [SerializeField]
    private bool invokeEvents;
    [SerializeField, EnableIf("invokeEvents")]
    private PropIntGameEvent eatEvent;

    public override void OnEquip(NewProp newProp)
    {
        // TEMP FIX
        int actualTotalBites = totalBites;

        //newProp.Stats.AddStat(new Stat());
        if (newProp.Stats.GetStat(biteType) != null)
        {
            newProp.Stats.AddStat(new Stat(actualTotalBites, biteType, newProp.Stats));
        }
        //else
        //{
        //    newProp.Stats.AddModifier(biteType, new StatModifier(StatModifierTypes.Flat, actualTotalBites));
        //}
    }

    public override bool TryUse(NewProp newProp)
    {
        if (newProp.Stats.GetStat(biteType) == null) { return false; }

        // Spread, just return true without doing anything
        if(newProp.TryGetComponent<Spread>(out Spread spread))
        {
            if(spread.IsOnKnife)
            {
                return true;
            }
        }

        TakeBite(newProp);

        return true;
    }

    private void TakeBite(NewProp newProp)
    {
        // play sound
        //eatAudioEvent.Play(audioSource);
        AudioManager.instance.PlayAudioEvent(eatSound);

        // play particles
        Camera.main.GetComponent<Hand>().PlayEatParticles(newProp.gameObject);

        // decrease bitesRemaining
        newProp.Stats.IncrementStat(biteType, -1);

        newProp.Stats.GetStat(biteType).UpdateValue();

        //// if bitesRemaining <= 0, then Eat
        //if (newProp.Stats.GetStat(biteType).Value == 0)
        //{
        //    EatWhole(newProp);
        //}
    }

    // MOVED TO EATEN ATTRIBUTE !!!!!!!
    //private void EatWhole(NewProp newProp)
    //{
    //    if (invokeEvents)
    //    {
    //        eatEvent.RaiseEvent(newProp, 1);
    //    }

    //    Destroy(newProp.gameObject);
    //}
}
