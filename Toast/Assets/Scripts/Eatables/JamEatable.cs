using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamEatable : MonoBehaviour, IEatable
{
    public int totalBites = 100;
    public int bitesRemaining = 100;
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

    public int BitesLeft()
    {
        return bitesRemaining;
    }
}
