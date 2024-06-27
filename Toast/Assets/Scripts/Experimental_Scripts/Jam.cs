using JetBrains.Annotations;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
// Author: Nick Kannnenberg
// Experimental Jam script, handles capping/uncapping

public class Jam : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Object References")]
    [SerializeField]
    GameObject jamJar;
    [SerializeField]
    GameObject jamJarLid;

    private bool isCapped = true;

    [Header("Prefab References")]
    [SerializeField]
    GameObject jamJarLidPrefab;

    [Header("Event References - Grabbed automatically, only change if there's a specific reason")]
    [SerializeField]
    private PropIntGameEvent capEvent;
    [SerializeField]
    private PropIntGameEvent uncapEvent;

    // ------------------------------- Properties -------------------------------
    public bool IsCapped { get => isCapped; set => isCapped = value; }

    [Button]
    private void CapJamLid() { CapJamNoLid(); }
    [Button]
    private void UncapJamLid() { UncapJam(); }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        if (isCapped && gameObject.GetComponent<NewProp>() != null)
        {
            gameObject.GetComponent<NewProp>().AddAttribute(PropFlags.JamLid);
        }
        if (!isCapped && gameObject.GetComponent<NewProp>() != null)
        {
            gameObject.GetComponent<NewProp>().RemoveAttribute(PropFlags.JamLid);
        }

        if(capEvent == null)
        {
            capEvent = PieManager.instance.CapObject;
        }
        if (uncapEvent == null)
        {
            uncapEvent = PieManager.instance.UncapObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Uncaps the Jam
    public void UncapJam()
    {
        if (isCapped)
        {
            isCapped = false;
            SetJamLidVisible(isCapped);
            GameObject newLid = GameObject.Instantiate(jamJarLidPrefab);
            newLid.transform.position = StationManager.instance.playerLocation.ObjectOffset;
            uncapEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), 1);
            gameObject.GetComponent<NewProp>()?.RemoveAttribute(PropFlags.JamLid);
        }
    }

    // Recaps the Jam, destroys cap object
    public void CapJam(GameObject lid)
    {
        if(!isCapped)
        {
            isCapped = true;
            SetJamLidVisible(isCapped);
            Destroy(lid);
            gameObject.GetComponent<NewProp>()?.AddAttribute(PropFlags.JamLid);
            capEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), 1);
        }
    }

    // Recaps the jam, does not require a lid object
    public void CapJamNoLid()
    {
        if (!isCapped)
        {
            isCapped = true;
            SetJamLidVisible(isCapped);
            gameObject.GetComponent<NewProp>()?.AddAttribute(PropFlags.JamLid);
        }
    }

    // Sets Jam Lid visibility
    private void SetJamLidVisible(bool visible)
    {
        jamJarLid.SetActive(visible);
    }

    // Caps the jam when a lid collides with the top of it
    private void OnTriggerEnter(Collider other)
    {
        NewProp oth = other.gameObject.GetComponent<NewProp>();
        if(oth != null)
        {
            if(oth.attributes.HasFlag(PropFlags.JamLid))
            {
                CapJam(other.gameObject);
            }
        }
    }
}
