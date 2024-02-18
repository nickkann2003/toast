using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour, IUseStrategy
{
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
        Debug.Log(gameObject);
    }

    private void EatWhole()
    {
        Destroy(gameObject);
    }

    private void TakeBite()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);
        bitesRemaining--;
        if (bitesRemaining <= 0)
        {
            EatWhole();
        }
    }
}
