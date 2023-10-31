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
        // If this is the held item, remove it from hand
        if (Camera.main.GetComponent<Hand>().CheckHeldItem(gameObject))
        {
            Camera.main.GetComponent<Hand>().RemoveItem();
        }
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
