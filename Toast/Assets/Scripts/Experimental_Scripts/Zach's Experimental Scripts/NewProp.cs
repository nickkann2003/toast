using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEditor.Rendering;
using NaughtyAttributes;
using System.Linq;

public class NewProp : MonoBehaviour
{

    // ------------------------------- Variables -------------------------------
    [SerializeField]
    private PropSO propSO;

    [SerializeField]
    private GameObject staticMesh;
    public GameObject StaticMesh {  get { return staticMesh; } }

    [NonSerialized]
    public IceConfig iceConfig;
    [NonSerialized]
    public PD_Rigidbody PD_Rb;

    [NonSerialized]
    public GameObject fireObject;
    [NonSerialized]
    public GameObject iceObject;

    [HorizontalLine(color: EColor.Gray)]
    [Header("------------ Attributes ------------")]
    // MAYBE REMOVE
    public PropFlags propFlags;
    [SerializeField]
    private List<PropAttributeSO> attributesList;
    

    [Header("------------ Stats ------------")]
    [SerializeField]
    private StatsSystem statsSystem = new StatsSystem();

    public StatsSystem Stats { get { return statsSystem; } }

    [Header("------------ UseEffects ------------")]
    [SerializeField]
    private List<UseEffectSO> useEffects;


    //[Header("------------ Fire Variables ------------")]
    private Color strongestStrength = Color.black;
    private Color initialColor;
    private Color colorOffset;

    //[SerializeField]
    //private Material baseMat;
    [HorizontalLine(color: EColor.Gray)]
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
    private PropAttributeSO attributeToGive;
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


    // Currently Toasting Checks
    private bool toasting = false;
    private float toastCd = 0f;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Grab initial color and set color variables   // CHANGE TO staticMesh.GetComponent
        initialColor = gameObject.GetComponentInChildren<Renderer>().material.color;
        colorOffset = strongestStrength - initialColor;

        float colorStrength = 0;

        if (Stats.GetStat(StatAttManager.instance.toastType) != null)
        {
            colorStrength = Stats.GetStat(StatAttManager.instance.toastType).Value;
        }

        if (colorStrength > 1)
        {
            colorStrength = 1;
        }

        // Set renderer color   // CHANGE TO staticMesh.GetComponent
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
        statsSystem.SetBaseProp(this);
        if (propSO != null)
        {
            propSO.PopulateProp(this);
        }

        UpdateRigidbody();
    }


    // ------------------------------- ATTRIBUTE METHODS -------------------------------
    // ------------------------------- ATTRIBUTE METHODS -------------------------------
    public void AddAttribute(PropAttributeSO att)
    {
        attributesList.Add(att);
        att.OnEquip(this);
    }
    public void RemoveAttribute(PropAttributeSO att)
    {
        att.OnRemove(this);
        attributesList.Remove(att);
    }
    public void RemoveAttributeWithoutOnRemove(PropAttributeSO att)
    {
        attributesList.Remove(att);
    }
    public bool HasAttribute(PropAttributeSO attributeToGet)
    {
        return attributesList.Contains(attributeToGet);
    }
    // ------------------------------- ATTRIBUTE METHODS -------------------------------
    // ------------------------------- ATTRIBUTE METHODS -------------------------------

    public void AddUseEffect(UseEffectSO useEffect)
    {
        useEffects.Add(useEffect);
        useEffect.OnEquip(this);
    }
    public void RemoveUseEffect(UseEffectSO useEffect)
    {
        useEffect.OnRemove(this);
        useEffects.Remove(useEffect);
    }
    public bool HasUseEffect(UseEffectSO useEffect)
    {
        return useEffects.Contains(useEffect);
    }

    // TryUse (input)

    public bool TryUse()
    {
        if (useEffects.Count <= 0) { return false; }

        for (int i = 0; i < useEffects.Count; i++)
        {
            if (!useEffects[i].TryUse(this))
            {
                return false;
            }
        }

        return true;

    }

    // Use this prop's strategy
    public virtual void Use()
    {
        if (useEffects.Count <= 0 ) { return; }

        for (int i = 0; i < useEffects.Count; i++)
        {
            useEffects[i].TryUse(this);
        }
    }

    // Force removes this item from hand
    public void ForceRemoveFromHand()
    {
        // If this item is in hand, then force hand to drop it
        if (HasAttribute(StatAttManager.instance.inHandAtt))
        {
            RemoveAttribute(StatAttManager.instance.inHandAtt);
            transform.parent.GetComponent<NewHand>()?.Drop();

            // If no rigidbody when dropped, get one
            if (this.GetComponent<Rigidbody>() == null)
            {
                CreateAndUpdateRigidbody();
            }
        }
    }

    // Add an attribute to this prop
    public void AddFlag(PropFlags flagToAdd)
    {
        propFlags |= flagToAdd;
    }

    // Remove an attribute from this prop
    public void RemoveFlag(PropFlags flagToRemove)
    {
        propFlags &= ~flagToRemove;
    }

    // Check if a prop has attributes
    public bool HasFlag(PropFlags flagToCheck)
    {
        return propFlags.HasFlag(flagToCheck);
    }

    // Increase toastiness by a given value
    public void IncreaseToastiness(float val, bool causeDelay = false)
    {
        // Increase toastiness
        //toastiness += val;

        float testToastiness = 0f;
        Stat toastStat = statsSystem.GetStat(StatAttManager.instance.toastType);
        if (toastStat != null)
        {
            toastStat.IncreaseValue(val);
            testToastiness = toastStat.UpdateValue();
        }

        // Get color strength and cap it
        float colorStrength = testToastiness;
        if (colorStrength > 0.95f)
        {
            colorStrength = 0.95f;
        }

        // Set renderer color
        gameObject.GetComponentInChildren<Renderer>().material.color = initialColor + (colorOffset * colorStrength);

        toasting = true;
        if (causeDelay)
        {
            toastCd = 0.2f;
        }
    }

    //private void RunEventChecks()
    //{
    //    // Adjust prop flags and trigger requirement events
    //    if (toastiness > .15f && !attributes.HasFlag(PropFlags.Toast)) // Toasted event
    //    {
    //        // Add attribtue
    //        AddAttribute(PropFlags.Toast);

    //        // Trigger Objectives
    //        if (toastObjectEvent != null)
    //            toastObjectEvent.RaiseEvent(this, 1);
    //    }

    //    if (toastiness > .9f && !attributes.HasFlag(PropFlags.Burnt)) // Burnt event
    //    {
    //        // Add attributes
    //        AddAttribute(PropFlags.Burnt);

    //        // Trigger Objectives
    //        if (burnObjectEvent != null)
    //            burnObjectEvent.RaiseEvent(this, 1);
    //    }
    //    if (!attributes.HasFlag(PropFlags.OnFire) && toastiness > fireTrigger && firePrefab != null) // On Fire event
    //    {
    //        // Instantiate fire
    //        GameObject fire = Instantiate(firePrefab);
    //        fire.transform.parent = gameObject.transform;
    //        fire.transform.localPosition = Vector3.zero;
    //        fire.transform.eulerAngles = Vector3.zero;
    //        fire.transform.localScale = Vector3.one;

    //        // Add attribute
    //        AddAttribute(PropFlags.OnFire);

    //        // Trigger objectives
    //        if (setObjectOnFireEvent != null)
    //            setObjectOnFireEvent.RaiseEvent(this, 1);

    //        // Add flaming object
    //        FireEndingManager.instance.addFireObject(gameObject);
    //    }
    //}

    // ------------------------------- RIGIDBODY METHODS -------------------------------
    // ------------------------------- RIGIDBODY METHODS -------------------------------

    public void UpdateRigidbody()
    {
        if (PD_Rb == null) return;

        PD_Rb.UpdateRigidbody(this);
    }

    public void CreateAndUpdateRigidbody()
    {
        if (this.GetComponent<Rigidbody>() == null)
        {
            this.AddComponent<Rigidbody>();
        }
        UpdateRigidbody();
    }

    public void RemoveRigidbody()
    {
        if (this.GetComponent<Rigidbody>() != null)
        {
            Destroy(this.GetComponent<Rigidbody>());
        }
    }

    public void RecalcSize()
    {
        Stat sizeStat = Stats.GetStat(StatAttManager.instance.sizeType);
        transform.localScale = Vector3.one * sizeStat.Value;
    }
    public void RecalcWeight()
    {
        Stat massStat = Stats.GetStat(StatAttManager.instance.massType);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = massStat.Value;
        }
    }
}
