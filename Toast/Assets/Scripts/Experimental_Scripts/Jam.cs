using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
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
            newLid.transform.position = StationManager.instance.playerLocation.objectOffset;
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
        }
    }

    // Recaps the jam, does not require a lid object
    public void CapJamNoLid()
    {
        if (!isCapped)
        {
            isCapped = true;
            SetJamLidVisible(isCapped);
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
            if(oth.propFlags.HasFlag(PropFlags.JamLid))
            {
                CapJam(other.gameObject);
            }
        }
    }
}
