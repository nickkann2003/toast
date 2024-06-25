using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRespawner : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("-------------- List of Objects to Respawn ---------------")]
    [SerializeField] public List<RespawnableObject> objects;

    [Header("------------- Spawn Variables -------------")]
    [SerializeField] public bool waitForAll = false;
    [SerializeField] public bool autoRespawnItems = true;
    [SerializeField] public bool spawnOnStart = true;

    private List<RespawnableObject> objectsToRespawn = new List<RespawnableObject>();

    // ------------------------------- Functions -------------------------------
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

    /// <summary>
    /// Respawns all items if they do not exist
    /// </summary>
    public void RespawnItems()
    {
        if(gameObject.activeSelf)
        {
            bool empty = true;
            // Innefficient, temp solution
            foreach (RespawnableObject obj in objects)
            {
                if (!obj.CheckNull() && !objectsToRespawn.Contains(obj))
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
            if (empty && waitForAll)
            {
                foreach (RespawnableObject obj in objects)
                {
                    obj.RespawnObject(transform.position);
                }
                objectsToRespawn.Clear();
            }
            if (!waitForAll)
            {
                objectsToRespawn.Clear();
            }
        }
    }

    /// <summary>
    /// Gizmos for each spawn position
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        foreach(RespawnableObject obj in objects)
        {
            Gizmos.DrawWireSphere(obj.spawnPosition + transform.position, 0.05f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach(RespawnableObject obj in objects)
        {
            if (other.gameObject == obj.ObjRef)
            {
                objectsToRespawn.Add(obj);
                Debug.Log("Added item to respawn list"); //DEBUG
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (RespawnableObject obj in objectsToRespawn)
        {
            if (other.gameObject == obj.ObjRef)
            {
                objectsToRespawn.Remove(obj);
                Debug.Log("Removed object from respawn list"); //DEBUG
            }
        }
    }
}

[Serializable]
public class RespawnableObject
{
    // ------------------------------- Variables -------------------------------
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 spawnPosition;
    [SerializeField] public Quaternion spawnRotation;
    private GameObject objRef = null;

    public GameObject ObjRef { get => objRef; set => objRef = value; }

    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Checks if this respawnable object is null
    /// </summary>
    /// <returns></returns>
    public bool CheckNull()
    {
        return objRef == null;
    }

    /// <summary>
    /// Respawns this object, taking its parents offset as a parameter
    /// </summary>
    /// <param name="parentOffset"></param>
    public void RespawnObject(Vector3 parentOffset)
    {
        objRef = GameObject.Instantiate(prefab);
        objRef.transform.position = spawnPosition + parentOffset;
        objRef.transform.rotation = spawnRotation;
    }
}