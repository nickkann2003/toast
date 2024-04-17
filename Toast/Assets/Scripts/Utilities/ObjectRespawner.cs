using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRespawner : MonoBehaviour
{
    [Header("-------------- List of Objects to Respawn ---------------")]
    [SerializeField] public List<RespawnableObject> objects;

    [Header("------------- Spawn Variables -------------")]
    [SerializeField] public bool waitForAll = false;
    [SerializeField] public bool autoRespawnItems = true;
    [SerializeField] public bool spawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.activeSelf && spawnOnStart)
        {
            foreach(RespawnableObject obj in objects)
            {
                if (obj.CheckNull())
                {
                    obj.RespawnObject(transform.position);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (autoRespawnItems)
        {
            RespawnItems();
        }
    }

    public void RespawnItems()
    {
        if(gameObject.activeSelf)
        {
            bool empty = true;
            // Innefficient, temp solution
            foreach (RespawnableObject obj in objects)
            {
                if (!obj.CheckNull())
                {
                    empty = false;
                }
                else
                {
                    if (!waitForAll)
                    {
                        obj.RespawnObject(transform.position);
                    }
                }
            }
            if (empty)
            {
                foreach (RespawnableObject obj in objects)
                {
                    obj.RespawnObject(transform.position);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach(RespawnableObject obj in objects)
        {
            Gizmos.DrawWireSphere(obj.spawnPosition + transform.position, 0.05f);
        }
    }
}

[Serializable]
public class RespawnableObject
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 spawnPosition;
    [SerializeField] public Quaternion spawnRotation;
    private GameObject objRef = null;

    public bool CheckNull()
    {
        return objRef == null;
    }

    public void RespawnObject(Vector3 parentOffset)
    {
        objRef = GameObject.Instantiate(prefab);
        objRef.transform.position = spawnPosition + parentOffset;
        objRef.transform.rotation = spawnRotation;
    }
}