using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadEatable : MonoBehaviour, IEatable
{
    public float totalBites = 1;
    public float bitesRemaining = 1;
    public bool IsEatable => true;

    public void EatWhole()
    {
        Destroy(gameObject);
    }

    public void TakeBite()
    {
        bitesRemaining--;
        if (bitesRemaining <= 0)
        {
            EatWhole();
        }
    }
}
