using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif // UNITY_EDITOR

public class PhysicalButtons : MonoBehaviour
{
    // ------------------------------- Enums -------------------------------
    public enum Trigger
    {
        onDown,
        onUp,
        onHold
    }

    // ------------------------------- Variables -------------------------------
    [Header("----------- Unity Events ------------")]
    [SerializeField] private UnityEvent buttonTrigger;

    [Header("----------- Height Entities -----------")]
    public GameObject maxHeight;
    public GameObject minHieght;

    // amount that the button has moved towards min height
    private float interpolateAmount;

    // timer
    [Header("------------ Time ------------")]
    public float maxTime;
    private float timer;

    public Trigger trigger;
    private bool pressed;

    [Header("------------ Audio ------------")]
    [SerializeField]
    private AudioEvent buttonDown;
    [SerializeField]
    private AudioEvent buttonUp;

    private AudioSource source1;
    private AudioSource source2;

    [SerializeField]
    private bool stickOnPress = false;
    private bool sticking = false;

    // ------------------------------- Functions -------------------------------
    private void Start()
    {
        timer = maxTime;

        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();

        source1.dopplerLevel = 0f;
        source1.spatialBlend = 1.0f;

        source2.dopplerLevel = 0f;
        source2.spatialBlend = 1.0f;
    }

    // On update, interact if pressed
    private void Update()
    {
        if (pressed)
        {
            Interact();
        }
        else
        {
            if (!sticking)
            {
                Depress();
            }
        }
    }

    // On click, interact
    private void OnMouseDown()
    {
        Interact();
        
    }

    // On release, activate if on-up
    private void OnMouseUp()
    {
        if (pressed)
        {
            buttonUp.Play(source2);
        }

        pressed = false;
        if (trigger == Trigger.onUp)
        {
            Activate();
        }
        timer = maxTime;
    }

    // Starts the interaction with this button
    void Interact()
    {
        if (!sticking)
        {
            Press();
            switch (trigger)
            {
                case Trigger.onDown:
                    if (!pressed)
                    {
                        Activate();
                    }
                    break;
                case Trigger.onHold:
                    Activate();
                    break;
            }

            // Play audio on first press
            if (!pressed)
            {
                buttonDown.Play(source1);
            }
            pressed = true;
            if (stickOnPress)
                sticking = true;
        }
    }

    // Activates the button
    void Activate()
    {
        if (timer >= maxTime)
        {
            buttonTrigger.Invoke();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void ForceActivate()
    {
        buttonTrigger.Invoke();
    }

    // pushes button down
    void Press()
    {
        if (interpolateAmount >= 0 && interpolateAmount < 1)
        {
            interpolateAmount += Time.deltaTime * 50;
            buttonDown.Play(source1);
        }
        else
        {
            interpolateAmount = 1;
        }
  
        transform.position = Vector3.Lerp(maxHeight.transform.position, minHieght.transform.position, interpolateAmount);
    }

    // Lerps button
    void Depress()
    {
        if (interpolateAmount > 0 && interpolateAmount <= 1)
        {
            interpolateAmount -= Time.deltaTime * 50;
            
        }
        else
        {
            interpolateAmount = 0;
        }

        transform.position = Vector3.Lerp(maxHeight.transform.position, minHieght.transform.position, interpolateAmount);
    }

    public void UnStick()
    {
        sticking = false;
    }
}