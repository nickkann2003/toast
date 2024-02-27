using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    private GameObject objectRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objectRef == null)
        {
            objectRef = Instantiate(prefab);
            objectRef.transform.position = spawnPosition + transform.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPosition + transform.position, 0.1f);
    }
}
