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

    //public FireEndingManager fireEndingManager;

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
    private bool defrost = false;

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
                    if(key == null)
                    {
                        continue;
                    }
                    NewProp prop = key.GetComponent<NewProp>();
                    if (prop != null)
                    {
                        if (!prop.attributes.HasFlag(PropFlags.Frozen))
                        {
                            prop.IncreaseToastiness((Time.deltaTime / maxTime) * targetStrength);
                        }
                        else if (defrost)
                        {
                            // if frozen toast slower
                            prop.IncreaseToastiness((Time.deltaTime / maxTime) * targetStrength/2f);
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
                if (obj.GetComponent<NewProp>() != null)
                {
                    toastiness = obj.GetComponent<NewProp>().toastiness;
                }
                ObjectVariables objVar = obj.GetComponent<ObjectVariables>();

                if (objVar != null && objVar.attributes.Contains(Attribute.Metal))
                {
                    //// METAL EXPLODE
                    //Vector3 explosionVel = new Vector3(Random.Range(-1,1), Random.value, 0);
                    //explosionVel *= 10;
                    //explosionVel += new Vector3((explosionVel.x / Mathf.Abs(explosionVel.x)) * 10, 10, 0);

                    //transform.GetComponent<Rigidbody>().velocity = explosionVel;
                }
                else if (!toastingObjects.ContainsKey(obj))
                {
                    toastingObjects.Add(obj, new ToastingObject(obj, toastiness, strongestStrength, weakestStrength, targetStrength));
                    obj.GetComponent<NewProp>().AddAttribute(PropFlags.ImmuneToDrag);
                    obj.GetComponent<NewProp>().AddAttribute(PropFlags.ImmuneToPickup);

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
                    NewProp prop = obj.GetComponent<NewProp>();

                    if (prop != null)
                    {
                        if(defrost)
                            prop.DefrostToast();

                        obj.GetComponent<NewProp>().RemoveAttribute(PropFlags.ImmuneToDrag);
                        obj.GetComponent<NewProp>().RemoveAttribute(PropFlags.ImmuneToPickup);
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

    public void toggleDefrost()
    {
        defrost = !defrost;
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
        if (renderer != null)
        {
            this.totalOffset = ((strongestStrength - weakestStrength) * colorStrength + weakestStrength) - renderer.material.color;
        }
    }

    public void adjustColor(float maxTime, float deltaTime)
    {
        float pChange = deltaTime / maxTime;
        renderer.material.color += totalOffset*pChange;
        toastiness += targetStrength * pChange;
    }
}
