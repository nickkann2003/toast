using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class ToastingBreadTest : MonoBehaviour
{
    [SerializeField] private UnityEvent startToasting;
    [SerializeField] private UnityEvent toasting;
    [SerializeField] private UnityEvent stopToasting;

    private List<GameObject> collidingObjects = new List<GameObject>();
    private float targetStrength = .5f;
    public Color weakestStrength = Color.white;
    public Color strongestStrength = Color.black;
    private bool isActive = false;

    private Dictionary<GameObject, ToastingObject> toastingObjects = new Dictionary<GameObject, ToastingObject>();
    public float timer;
    public float maxTime;

    public ParticleSystem smokeParticles;

    public bool IsActive { get => isActive; }

    public void Update()
    {
        if (isActive)
        {
            if (toastingObjects.Count > 0)
            {
                foreach (var (key, value) in toastingObjects)
                {
                    ToastingObject toast = value;
                    toast.adjustColor(maxTime, Time.deltaTime);
                }
            }
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                deactivateTrigger();
            }
        }
    }

    public void activateTrigger()
    {
        timer = maxTime;
        Color targetColor = (strongestStrength - weakestStrength) * targetStrength + weakestStrength;
        foreach (GameObject obj in collidingObjects)
        {
            if (obj != null)
            {
                Color totalOffset = targetColor - obj.GetComponent<Renderer>().material.color;
                if (!toastingObjects.ContainsKey(obj))
                {
                    toastingObjects.Add(obj, new ToastingObject(obj.GetComponent<Renderer>(), totalOffset));
                }
            }
        }
        EmissionModule em = smokeParticles.emission;
        em.rateOverTimeMultiplier = 0.3f + (6 * targetStrength);
        smokeParticles.Play();
        isActive = true;

        startToasting.Invoke();
    }

    public void deactivateTrigger()
    {
        if (isActive)
        {
            timer = 0;
            toastingObjects.Clear();
            isActive = false;
            stopToasting.Invoke();
        }
    }

    public void setDialValue(float value)
    {
        targetStrength = value;
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if(!collidingObjects.Contains(other.gameObject))
                collidingObjects.Add(other.gameObject);
        }
        catch
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            if (collidingObjects.Contains(other.gameObject))
                collidingObjects.Remove(other.gameObject);
            
            if (toastingObjects.ContainsKey(other.gameObject))
                toastingObjects.Remove(other.gameObject);
            
            
        }
        catch
        {
            return;
        }
    }
}

class ToastingObject {
    Renderer renderer;
    Color totalOffset;

    public ToastingObject(Renderer renderer, Color totalOffset)
    {
        this.renderer = renderer;
        this.totalOffset = totalOffset;
    }

    public void adjustColor(float maxTime, float deltaTime)
    {
        float pChange = deltaTime / maxTime;
        renderer.material.color += totalOffset*pChange;
    }
}