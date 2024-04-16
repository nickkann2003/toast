using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour, IUseStrategy
{
    public int totalBites;
    public int bitesRemaining;
    private NewProp propScript;

    void Start()
    {
        if (propScript == null)
        {
            propScript = this.gameObject.GetComponent<NewProp>();
        }
    }

    public void Use()
    {
        TakeBite();
    }

    private void EatWhole()
    {
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.EatObject, propScript.attributes, true));
        Destroy(gameObject);
    }

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
