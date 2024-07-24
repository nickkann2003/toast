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

    [SerializeField]
    PropAttributeSO lidAtt;

    [SerializeField]
    private JamConfig config;
    public JamConfig Config { get { return config; } }

    [SerializeField, ReadOnly]
    private bool isCapped = true;

    [Header("Prefab References")]
    [SerializeField]
    GameObject jamJarLidPrefab;

    [SerializeField]
    private NewHand newHand;

    //public NewHand Hand { get { return newHand; } }

    [Header("Event References - Grabbed automatically, only change if there's a specific reason")]
    [SerializeField]
    private PropIntGameEvent capEvent;
    [SerializeField]
    private PropIntGameEvent uncapEvent;

    // ------------------------------- Properties -------------------------------
    public bool IsCapped
    {
        get
        {
            if (newHand.CheckObject())
            {
                isCapped = true;
            }
            else
            {
                isCapped = false;
            }

            return isCapped;
        }
    }

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
            gameObject.GetComponent<NewProp>().AddFlag(PropFlags.JamLid);
        }
        if (!isCapped && gameObject.GetComponent<NewProp>() != null)
        {
            gameObject.GetComponent<NewProp>().RemoveFlag(PropFlags.JamLid);
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
        if (IsCapped)
        {
            isCapped = false;
            GameObject lid = newHand.Drop();
            AudioManager.instance.PlayOneShotSound(AudioManager.instance.jamOpen);
            if (lid != null)
            {
                // CHANGE LATER
                lid.transform.position = StationManager.instance.playerLocation.ObjectOffset;
            }
            uncapEvent.RaiseEvent(gameObject.GetComponentInParent<NewProp>(), 1);

            //SetJamLidVisible(isCapped);
            //GameObject newLid = GameObject.Instantiate(jamJarLidPrefab);
            //newLid.transform.position = StationManager.instance.playerLocation.ObjectOffset;
            //uncapEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), 1);
            //gameObject.GetComponent<NewProp>()?.RemoveFlag(PropFlags.JamLid);
        }
    }

    // Recaps the Jam, destroys cap object
    public void CapJam(GameObject lid)
    {
        if(!IsCapped)
        {
            isCapped = true;
            newHand.Pickup(lid);
            capEvent.RaiseEvent(gameObject.GetComponentInParent<NewProp>(), 1);

            //SetJamLidVisible(isCapped);
            //Destroy(lid);
            //gameObject.GetComponent<NewProp>()?.AddFlag(PropFlags.JamLid);
            //capEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), 1);
        }
    }

    // Recaps the jam, does not require a lid object
    public void CapJamNoLid()
    {
        if (!IsCapped)
        {
            isCapped = true;
            SetJamLidVisible(isCapped);
            gameObject.GetComponent<NewProp>()?.AddFlag(PropFlags.JamLid);
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
        NewProp prop = other.gameObject.GetComponent<NewProp>();
        if(prop != null)
        {
            if (prop.HasAttribute(lidAtt))
            {
                CapJam(other.gameObject);
            }
        }
    }
}
