using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class ToastingBreadTest : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Unity Events")]
    [SerializeField] private UnityEvent startToasting;
    [SerializeField] private UnityEvent toasting;
    [SerializeField] private UnityEvent stopToasting;

    [Header("Colors")]
    [SerializeField] private Color weakestStrength = Color.white;
    [SerializeField] private Color strongestStrength = Color.black;

    private List<GameObject> collidingObjects = new List<GameObject>();
    private float targetStrength = .5f;
    private bool isActive = false;

    private Dictionary<GameObject, ToastingObject> toastingObjects = new Dictionary<GameObject, ToastingObject>();

    [Header("Time and Toast Variables")]
    [SerializeField] public float timer;
    [SerializeField] public float maxTime;
    [SerializeField] float fireTrigger = 1.5f;
    private float baseTime;

    [Header("References")]
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Electricity electricity;

    [SerializeField] private bool customEvents = false;
    [Header("PIE Event References"), ShowIf("customEvents")]
    [SerializeField] private PropIntGameEvent toastStrength1;
    [SerializeField, ShowIf("customEvents")] private PropIntGameEvent toastStrength2;
    [SerializeField, ShowIf("customEvents")] private PropIntGameEvent toastStrength3;
    [SerializeField, ShowIf("customEvents")] private PropIntGameEvent toastStrength4;
    [SerializeField, ShowIf("customEvents")] private PropIntGameEvent toastStrength5;

    [Header("Audio")]
    [SerializeField]
    private SimpleAudioEvent sizzleAudio;
    [SerializeField]
    private SimpleAudioEvent toasterTicking;
    [SerializeField]
    private SimpleAudioEvent toasterStart;
    [SerializeField]
    private SimpleAudioEvent toasterEnd;
    [SerializeField]
    private SimpleAudioEvent toasterPop;

    private AudioSource source1;
    private AudioSource source2;

    private bool defrost = false;

    private bool bagel = false;

    private int snapPoint;

    // ------------------------------- Properties -------------------------------
    public bool IsActive { get => isActive; }


    // ------------------------------- Functions -------------------------------
    public void Awake()
    {
        baseTime = maxTime;
    }

    private void Start()
    {
        if(toastStrength1 == null)
            toastStrength1 = PieManager.instance.ToastStrength1;
        if (toastStrength2 == null)
            toastStrength2 = PieManager.instance.ToastStrength2;
        if (toastStrength3 == null)
            toastStrength3 = PieManager.instance.ToastStrength3;
        if (toastStrength4 == null)
            toastStrength4 = PieManager.instance.ToastStrength4;
        if (toastStrength5 == null)
            toastStrength5 = PieManager.instance.ToastStrength5;

        if(electricity == null)
        {
            electricity = gameObject.GetComponent<Electricity>();
        }

        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();

        source1.spatialBlend = 1.0f;
        source2.spatialBlend = 1.0f;
        source1.dopplerLevel = 0.0f;
        source2.dopplerLevel = 0.0f;
    }

    public void Update()
    {
        // Check Active, has objects, then loop all objects
        if (isActive)
        {
            if (toastingObjects.Count > 0)
            {
                foreach (var (key, value) in toastingObjects)
                {
                    // Check object null
                    ToastingObject toast = value;
                    if(key == null || key.GetComponent<Rigidbody>() == null)
                    {
                        continue;
                    }

                    // Get prop and check null
                    NewProp prop = key.GetComponent<NewProp>();
                    if (prop != null)
                    {
                        // If not frozen, then toast
                        if (!prop.propFlags.HasFlag(PropFlags.Frozen))
                        {
                            prop.IncreaseToastiness((Time.deltaTime / maxTime) * targetStrength, true);
                        }
                        else if (defrost)
                        {
                            // If frozen and defrosting, toast slowly
                            prop.IncreaseToastiness((Time.deltaTime / maxTime) * targetStrength/2f, true);
                        }
                    }
                }
            }

            // Decrement time
            timer -= Time.deltaTime;

            // Reset if timer is 0
            if (timer <= 0)
            {
                deactivateTrigger();
            }
        }
    }

    // Starts toasting
    public void activateTrigger()
    {
        // Set timer to max time
        timer = maxTime;

        toasterStart.Play(source1);
        toasterTicking.Play(source2);

        // Get toasting objects from colliding objects
        foreach (GameObject obj in collidingObjects)
        {
            // If object isn't null
            if (obj != null)
            {
                // Set toastiness
                float toastiness = 0.0f;
                if (obj.GetComponent<NewProp>() != null)
                {
                    Stat toastStat = obj.GetComponent<NewProp>().Stats.GetStat(StatAttManager.instance.toastType);
                    if (toastStat != null)
                    {
                        toastiness = toastStat.Value;
                    }
                }

                // Get attributes and check specific interactions
                if (!toastingObjects.ContainsKey(obj))
                {
                    toastingObjects.Add(obj, new ToastingObject(obj, toastiness, strongestStrength, weakestStrength, targetStrength));
                }
            }
        }
        // Set color and particles
        var main = smokeParticles.main;
        Color light = new Color(100, 100, 100);
        Color burnStrength = new Color(150 * targetStrength, 150 * targetStrength, 150 * targetStrength);
        EmissionModule em = smokeParticles.emission;
        em.rateOverTimeMultiplier = 0.3f + (6 * targetStrength);
        smokeParticles.Play();
        isActive = true;

        // Start toastig event
        startToasting.Invoke();

        // Turn on electricity
        electricity.PoweredOn = true;
    }

    // Deactivates toasting
    public void deactivateTrigger()
    {
        // If active, loop all objs not null
        if (isActive)
        {
            source1.Stop();
            source2.Stop();

            toasterEnd.Play(source1);
            toasterPop.Play(source2);

            foreach (GameObject obj in collidingObjects)
            {
                if (obj != null)
                {
                    //Get prop, set flags
                    NewProp prop = obj.GetComponent<NewProp>();
                    if (prop != null)
                    {
                        prop.PlayAudioEvent(sizzleAudio);

                        if (defrost)
                            prop.RemoveAttribute(StatAttManager.instance.frozenAtt);

                        if(bagel)
                        {
                            prop.AddAttribute(StatAttManager.instance.bagelAtt);
                        }

                        switch (snapPoint)
                        {
                            case 0:
                                break;
                            case 1:
                                toastStrength1.RaiseEvent(prop, 1);
                                break;
                            case 2:
                                toastStrength2.RaiseEvent(prop, 1);
                                break;
                            case 3:
                                toastStrength3.RaiseEvent(prop, 1);
                                break;
                            case 4:
                                toastStrength4.RaiseEvent(prop, 1);
                                break;
                            case 5:
                                toastStrength5.RaiseEvent(prop, 1);
                                break;

                        }
                    }


                    // Apply force to objs
                    Rigidbody rb = obj.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 force = new Vector3(0, 5, 0);
                        force.y = 2.5f + 2 * (1 - (timer / maxTime));

                        rb.velocity += force;
                    }
                }
            }
            // Reset timer and data structures
            timer = 0;
            toastingObjects.Clear();
            isActive = false;
            smokeParticles.Stop();
            stopToasting.Invoke();

            electricity.PoweredOn = false;
        }
    }

    // Toggles the defrost setting
    public void toggleDefrost()
    {
        defrost = !defrost;
    }

    /// <summary>
    /// Toggles the bagel setting
    /// </summary>
    public void toggleBagel()
    {
        bagel = !bagel;
    }

    // Sets the toasting dials value
    public void setDialValue(float value)
    {
        targetStrength = value;
        maxTime = baseTime + (targetStrength - .5f) * baseTime; // between -baseTime/2 and baseTime/2
    }

    public void setDialSnap(int snapPoint)
    {
        this.snapPoint = snapPoint;
    }

    // When entering trigger, add to toasting object list
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

    // When exiting, try removing from list
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

// Toasting object class
class ToastingObject
{
    // ------------------------------- Variables -------------------------------
    private GameObject obj;
    private Renderer renderer;
    private Color totalOffset;
    private float targetStrength;
    
    public float toastiness;

    // ------------------------------- Constructors -------------------------------
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

    // ------------------------------- Functions -------------------------------
    // Adjust color based on a max time and delta time
    public void adjustColor(float maxTime, float deltaTime)
    {
        float pChange = deltaTime / maxTime;
        renderer.material.color += totalOffset*pChange;
        toastiness += targetStrength * pChange;
    }
}
