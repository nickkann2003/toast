using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIf : MonoBehaviour
{
    public int numTrigger = 2;
    private int hits = 0;

    private void Start()
    {
        gameObject.SetActive(false);
        hits = 0;
    }

    public void increment()
    {
        hits += 1;
        if (hits >= numTrigger)
            gameObject.SetActive(true);
    }
}
