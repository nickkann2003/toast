using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour, IUseStrategy
{
    // ------------------------------- Variables -------------------------------
    [Header("Eating Variables")]
    [SerializeField]
    private int totalBites;
    [SerializeField]
    private int bitesRemaining;
    
    private NewProp propScript;

    [SerializeField]
    PropIntGameEvent eatEvent;

    // ------------------------------- Functions -------------------------------
    // Run on start
    void Start()
    {
        if (propScript == null)
        {
            propScript = this.gameObject.GetComponent<NewProp>();
        }
    }

    // Called when the Object is used
    public void Use()
    {
        TakeBite();
    }

    // Eats the entire object and destroys it
    private void EatWhole()
    {
        eatEvent.RaiseEvent(propScript, 1);
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.EatObject, propScript.attributes, true));
        Destroy(gameObject);
    }

    // Takes a bit. May trigger Eat Whole if one bite is left
    private void TakeBite()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);
        Camera.main.GetComponent<Hand>().PlayEatParticles(gameObject);
        bitesRemaining--;
        if (bitesRemaining <= 0)
        {
            EatWhole();
        }
    }
}
