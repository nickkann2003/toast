using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Material M_Off;
    public Material M_On;

    Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        TurnOff();
    }

    public void TurnOn()
    {
        if (M_On != null)
        {
            GetComponent<Renderer>().material = M_On;
        }
    }

    public void TurnOff()
    {
        if (M_Off != null)
        {
            GetComponent<Renderer>().material = M_Off;
        }
    }
}
