using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Lighting Materials")]
    [SerializeField]
    public Material M_Off;
    [SerializeField]
    public Material M_On;

    private bool on;

    private Renderer meshRenderer;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        TurnOff();
    }

    // Toggles the light
    public void Toggle()
    {
        if (on)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    // Turns the light on
    public void TurnOn()
    {
        if (M_On != null)
        {
            GetComponent<Renderer>().material = M_On;
        }
        on = true;
    }

    // Turns the light off
    public void TurnOff()
    {
        if (M_Off != null)
        {
            GetComponent<Renderer>().material = M_Off;
        }
        on = false;
    }
}
