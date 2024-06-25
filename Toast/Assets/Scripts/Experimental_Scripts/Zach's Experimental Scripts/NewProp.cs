using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEditor.Rendering;
using NaughtyAttributes;

public class NewProp : MonoBehaviour
{

    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private PropSO propObject;

    [NonSerialized]
    public GameObject fireObject;

    [Header("------------ Attributes ------------")]
    public PropFlags propFlags;
    public List<PropAttributeObject> attributesList;

    [Header("------------ Stats ------------")]
    //[SerializeField]
    //private List<Stat> stats = new List<Stat>();
    [SerializeField]
    private StatsSystem statsSystem = new StatsSystem();

    public StatsSystem Stats { get { return statsSystem; } }

    [SerializeField]
    private StatType toastType;
    [SerializeField]
    private StatType sizeType;
    [SerializeField]
    private StatType massType;

    [Header("------------ UseEffects ------------")]
    [SerializeField]
    private List<UseEffectSO> useEffects;

    [HorizontalLine(color: EColor.Gray)]


    // Toast Values
    [Header("------------ Toastiness ------------")]
    public float toastiness;

    [Header("------------ Fire Variables ------------")]
    public float fireTrigger = 1.5f;
    public GameObject firePrefab;
    private float targetStrength = 0.5f;
    private Color strongestStrength = Color.black;
    private Color initialColor;
    private Color colorOffset;

    [Header("------------ Freeze Variables ------------")]
    public float frozenness;

    [SerializeField]
    private Material baseMat;

    protected IUseStrategy _useStrategy;

    [Header("------------ Prop Data ------------")]
    [SerializeField]
    private PD_Rigidbody PD_Rb;

    [Header("Event References - Only set if purposefully changing them from the norm")]
    [SerializeField]
    private PropIntGameEvent createObjectEvent;
    [SerializeField]
    private PropIntGameEvent thawObjectEvent;
    [SerializeField]
    private PropIntGameEvent toastObjectEvent;
    [SerializeField]
    private PropIntGameEvent burnObjectEvent;
    [SerializeField]
    private PropIntGameEvent setObjectOnFireEvent;

    [Header("------------ TESTING ------------")]
    [SerializeField]
    private PropAttributeObject attributeToGive;
    [SerializeField, Button]
    private void GiveAttribute()
    {
        attributesList.Add(attributeToGive);
        attributeToGive.OnEquip(this);
    }
    [SerializeField]
    private int indexToRemove = 0;
    [SerializeField, Button]
    private void RemoveAttributeAtIndex()
    {
        if (attributesList.Count < 0 || indexToRemove >= attributesList.Count) { return; }

        attributesList[indexToRemove]?.OnRemove(this);
        attributesList.RemoveAt(indexToRemove);
    }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        

        // Get use strategy on start
        _useStrategy = this.gameObject.GetComponent<IUseStrategy>();

        // Grab initial color and set color variables
        initialColor = gameObject.GetComponentInChildren<Renderer>().material.color;
        colorOffset = strongestStrength - initialColor;
        
        // If fire prefab not given, get it from manager
        if(firePrefab == null)
        {
            firePrefab = FireEndingManager.instance.firePrefab;
        }

        CreateAndUpdateRigidbody();

        float colorStrength = toastiness;
        if (colorStrength > 1)
        {
            colorStrength = 1;
        }

        // Set renderer color
        gameObject.GetComponentInChildren<Renderer>().material.color = initialColor + (colorOffset * colorStrength);

        if(createObjectEvent == null)
            createObjectEvent = PieManager.instance.CreateObject;
        if (thawObjectEvent == null)
            thawObjectEvent = PieManager.instance.ThawObject;
        if (toastObjectEvent == null)
            toastObjectEvent = PieManager.instance.ToastObject;
        if (burnObjectEvent == null)
            burnObjectEvent = PieManager.instance.BurnObject;
        if (setObjectOnFireEvent == null)
            setObjectOnFireEvent = PieManager.instance.SetObjectOnFire;
    }

    private void OnEnable()
    {
        if (propObject != null)
        {
            propObject.PopulateProp(this);
            if (useEffects.Count  > 0)
            {
                for (int i = 0; i < useEffects.Count; i++)
                {
                    useEffects[i].OnEquip(this);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If in hand and has a rigid body, destroy rigidbody
        if (propFlags.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.GetComponent<Rigidbody>());
        }

        // If not in hand and doesn't have a rigid body, give it a rigidbody
        if (!propFlags.HasFlag(PropFlags.InHand) && this.GetComponent<Rigidbody>() == null)
        {
            CreateAndUpdateRigidbody();
        }
    }
    
    public void RecalcSize()
    {
        Stat sizeStat = Stats.GetStat(sizeType);
        transform.localScale = Vector3.one * sizeStat.Value;
    }

    public void RecalcWeight()
    {
        Stat massStat = Stats.GetStat(massType);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = massStat.Value;
        }
    }

    public bool TryUse()
    {
        if (useEffects.Count <= 0) { return false; }

        for (int i = 0; i < useEffects.Count; i++)
        {
            useEffects[i].Use(this);
        }

        return true;
    }

    // Use this prop's strategy
    public virtual void Use()
    {
        if (useEffects.Count <= 0 ) { return; }

        for (int i = 0; i < useEffects.Count; i++)
        {
            useEffects[i].Use(this);
        }
    }

    // Force removes this item from hand
    public void ForceRemoveFromHand()
    {
        // If this item is in hand, then force hand to drop it
        if (HasAttribute(PropFlags.InHand))
        {
            transform.parent.GetComponent<NewHand>()?.Drop();

            // If no rigidbody when dropped, get one
            if (this.GetComponent<Rigidbody>() == null)
            {
                CreateAndUpdateRigidbody();
            }
        }
    }

    // Add an attribute to this prop
    public void AddAttribute(PropFlags flagToAdd)
    {
        propFlags |= flagToAdd;
    }

    // Remove an attribute from this prop
    public void RemoveAttribute(PropFlags flagToRemove)
    {
        propFlags &= ~flagToRemove;
    }

    // Check if a prop has attributes
    public bool HasAttribute(PropFlags flagToCheck)
    {
        return propFlags.HasFlag(flagToCheck);
    }

    // Increase toastiness by a given value
    public void IncreaseToastiness(float val)
    {
        // Increase toastiness
        toastiness += val;

        float testToastiness = 0f;
        if (toastType != null)
        {
            statsSystem.IncrementStat(toastType, val);
            testToastiness = statsSystem.GetStat(toastType).Value;
        }

        // Get color strength and cap it
        float colorStrength = testToastiness;
        if (colorStrength > 1)
        {
            colorStrength = 1;
        }

        // Set renderer color
        gameObject.GetComponentInChildren<Renderer>().material.color = initialColor + (colorOffset * colorStrength);

        // Adjust prop flags and trigger requirement events
        if (testToastiness > .15f && !propFlags.HasFlag(PropFlags.Toast)) // Toasted event
        {
            // Add attribtue
            AddAttribute(PropFlags.Toast);

            // Trigger Objectives
            if(toastObjectEvent != null)
                toastObjectEvent.RaiseEvent(this, 1);
        }

        if (testToastiness > .9f && !propFlags.HasFlag(PropFlags.Burnt)) // Burnt event
        {
            // Add attributes
            AddAttribute(PropFlags.Burnt);

            // Trigger Objectives
            if(burnObjectEvent != null)
                burnObjectEvent.RaiseEvent(this, 1);
        }
        if (!propFlags.HasFlag(PropFlags.OnFire) && testToastiness > fireTrigger && firePrefab != null) // On Fire event
        {
            if (firePrefab == null)
            {
                FireEndingManager.instance.firePrefab = firePrefab;
            }

            // Instantiate fire
            GameObject fire = Instantiate(firePrefab);
            fire.transform.parent = gameObject.transform;
            fire.transform.localPosition = Vector3.zero;
            fire.transform.eulerAngles = Vector3.zero;
            fire.transform.localScale = Vector3.one;

            fireObject = fire;

            // Add attribute
            AddAttribute(PropFlags.OnFire);

            // Trigger objectives
            if(setObjectOnFireEvent != null)
                setObjectOnFireEvent.RaiseEvent(this, 1);
            
            // Add flaming object
            FireEndingManager.instance.addFireObject(gameObject);
        }
    }

    // Defrosts this object
    public void DefrostToast()
    {
        // If frozen, destroy top game object and remove frozen flag
        if (propFlags.HasFlag(PropFlags.Frozen))
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
            RemoveAttribute(PropFlags.Frozen);
            if(thawObjectEvent != null)
                thawObjectEvent.RaiseEvent(this, 1);
        }
    }

    public void UpdateRigidbody()
    {
        if (PD_Rb == null) return;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb == null) return;

        rb.mass = PD_Rb.mass;
        rb.drag = PD_Rb.drag;
        rb.angularDrag = PD_Rb.angularDrag;
        rb.useGravity = PD_Rb.useGravity;
        rb.collisionDetectionMode = PD_Rb.collisionDetection;
    }

    public void CreateAndUpdateRigidbody()
    {
        if (this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
        UpdateRigidbody();
    }
}
