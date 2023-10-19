using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Test scripts to cause environment effects, TODO: Replace this with a list of interactables that are triggered on specific events
    public ToastingBreadTest toastCollider;
    public SpawnPrefabScript spawnPrefab;

    // position in world
    public GameObject maxHeight;
    public GameObject minHieght;

    // amount that the button has moved towards min height
    private float interpolateAmount;

    // timer
    public float timer;
    public float maxTime;

    public enum Trigger
    {
        onDown,
        onUp,
        onHold
    }

    public Trigger trigger;
    private bool pressed;

    private void Start()
    {
        timer = maxTime;
    }

    private void Update()
    {
        if (pressed)
        {
            Interact();
        }
        else
        {
            Depress();
        }
    }

    private void OnMouseDown()
    {
        Interact();
    }

    private void OnMouseUp()
    {
        pressed = false;
        if (trigger == Trigger.onUp)
        {
            Activate();
        }
        timer = maxTime;
    }

    void Interact()
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
        pressed = true;
    }

    void Activate()
    {
        if (timer >= maxTime)
        {
            if (spawnPrefab != null)
            {
                spawnPrefab.TriggerSpawn();
            }
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    // pushes button down
    void Press()
    {
        if (interpolateAmount >= 0 && interpolateAmount < 1)
        {
            interpolateAmount += Time.deltaTime * 50;
        }
        else
        {
            interpolateAmount = 1;
        }

        transform.position = Vector3.Lerp(maxHeight.transform.position, minHieght.transform.position, interpolateAmount);
    }

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
}
