using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float baseTime;
    public float maxTime;

    public ParticleSystem smokeParticles;

    public GameObject firePrefab;
    public float fireTrigger = 1.5f;

    public bool IsActive { get => isActive; }

    public void Awake()
    {
        baseTime = maxTime;
    }

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
                    Prop prop = key.GetComponent<Prop>();
                    if (prop != null)
                    {
                        prop.toastiness = toast.toastiness;
                        ObjectVariables objVars = key.GetComponent<ObjectVariables>();
                        if (objVars != null)
                        {
                            if (!objVars.attributes.Contains(Attribute.OnFire) && prop.toastiness > fireTrigger)
                            {
                                GameObject fire = Instantiate(firePrefab);
                                fire.transform.parent = key.transform;
                                fire.transform.localPosition = Vector3.zero;
                                fire.transform.eulerAngles = Vector3.zero;
                                fire.transform.localScale = Vector3.one;
                                objVars.AddAttribute(Attribute.OnFire);
                            }
                        }
                        
                    }
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
        //Color targetColor = (strongestStrength - weakestStrength) * targetStrength + weakestStrength;
        foreach (GameObject obj in collidingObjects)
        {
            if (obj != null)
            {
                float toastiness = 0.0f;
                if (obj.GetComponent<Prop>() != null)
                {
                    toastiness = obj.GetComponent<Prop>().toastiness;
                }
                if (!toastingObjects.ContainsKey(obj))
                {
                    toastingObjects.Add(obj, new ToastingObject(obj, toastiness, strongestStrength, weakestStrength, targetStrength));
                }
            }
        }
        var main = smokeParticles.main;
        Color light = new Color(100, 100, 100);
        Color burnStrength = new Color(150 * targetStrength, 150 * targetStrength, 150 * targetStrength);
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
            foreach (GameObject obj in collidingObjects)
            {
                if (obj != null)
                {
                    if (targetStrength >= 0.15)
                    {
                        ObjectVariables objVars = obj.GetComponent<ObjectVariables>();
                        if (objVars != null && !objVars.attributes.Contains(Attribute.Toast))
                        {
                            objVars.AddAttribute(Attribute.Toast);
                            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.CreateObject, objVars, true));
                        }
                    }

                    // apply force to obj
                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 force = new Vector3(0, 5, 0);
                        force.y = 2.5f + 2 * (1 - (timer / maxTime));

                        rb.velocity += force;
                        print(force);
                    }
                }
            }
            timer = 0;
            toastingObjects.Clear();
            isActive = false;
            smokeParticles.Stop();
            stopToasting.Invoke();
        }
    }

    public void setDialValue(float value)
    {
        targetStrength = value;
        maxTime = baseTime + (targetStrength - .5f) * baseTime; // between -baseTime/2 and baseTime/2
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
    GameObject obj;
    Renderer renderer;
    Color totalOffset;
    float targetStrength;
    public float toastiness;

    public ToastingObject(GameObject obj, float toastiness, Color strongestStrength, Color weakestStrength, float targetStrength)
    {
        this.obj = obj;
        renderer = obj.GetComponent<Renderer>();
        this.targetStrength = targetStrength;
        this.toastiness = toastiness;
        float colorStrength = targetStrength + toastiness;
        if (colorStrength > 1)
        {
            colorStrength = 1;
        }
        this.totalOffset = ((strongestStrength - weakestStrength) * colorStrength + weakestStrength) - renderer.material.color;
    }

    public void adjustColor(float maxTime, float deltaTime)
    {
        float pChange = deltaTime / maxTime;
        renderer.material.color += totalOffset*pChange;
        toastiness += targetStrength * pChange;
    }
}
