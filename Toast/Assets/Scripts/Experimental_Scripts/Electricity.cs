using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    [SerializeField]
    List<ParticleSystem> particles;

    [SerializeField]
    float radius, power;

    [SerializeField]
    Station endingStation;

    LayerMask mask;

    [SerializeField]
    private bool metalInserted;

    private bool poweredOn;

    public bool PoweredOn { get { return poweredOn; } set { poweredOn = value; } }


    // Start is called before the first frame update
    void Start()
    {
        metalInserted = false;
        poweredOn = false;
        mask = LayerMask.GetMask("Interactable");
    }

    // Update is called once per frame
    void Update()
    {
        // Electricity is on and metal is inserted, explode
        if(poweredOn && metalInserted)
        {
            TriggerExplosion();
            metalInserted = false;
        }
    }


 
    private void OnTriggerEnter(Collider other)
    {
        // If prop is metal, set that metal is inserted
        if (other.gameObject.TryGetComponent(out NewProp prop))
        {
            if (prop.attributes.HasFlag(PropFlags.Metal))
            {
                metalInserted = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If prop is metal, set that metal is removed
        if (other.gameObject.TryGetComponent(out NewProp prop))
        {
            if (prop.attributes.HasFlag(PropFlags.Metal))
            {
                metalInserted = false;
            }
        }
    }

    private void TriggerExplosion()
    {
        // Play visual
        foreach (ParticleSystem p in particles)
        {

            p.Play();
        }

        // Add force
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius, mask);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

        // Play ending "animation"
        Time.timeScale = 0.2f;

        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(endingStation);

        StartCoroutine(GiveEnding());

    }

    /// <summary>
    /// Draw the radius of the explosion force
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private IEnumerator GiveEnding()
    {
        yield return new WaitForSecondsRealtime(3);

        GameManager.Instance.LoadGame(0);
    }
}
