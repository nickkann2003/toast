using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal.Internal;

public class ObjectRespawner : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("-------------- List of Objects to Respawn ---------------")]
    [SerializeField] public List<RespawnableObject> objects;

    [Header("------------- Spawn Variables -------------")]
    [SerializeField] public bool waitForAll = false;
    [SerializeField] public bool autoRespawnItems = true;
    [SerializeField] public bool spawnOnStart = true;
    [SerializeField] private GameObject spawnParent;

    /// <summary>
    /// Option respawn trigger collider, any object with this script and a trigger collider will automatically function off of this
    /// </summary>
    private BoxCollider respawnCollider;
    private LayerMask mask; // Layer mask for masking everything but interactable objects

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        if (spawnParent != null)
        {
            foreach (RespawnableObject obj in objects)
            {
                obj.SetSpawnParent(spawnParent);
            }
        }
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

        respawnCollider = null;
        if(gameObject.GetComponent<BoxCollider>() != null)
        {
            BoxCollider[] allColliders = gameObject.GetComponents<BoxCollider>();
            foreach(BoxCollider c in allColliders)
            {
                if (c.isTrigger)
                {
                    // Loop all colliders of this object, take the first trigger collider as the respawn collider
                    respawnCollider = c;
                    break;
                }
            }
        }
        mask = LayerMask.GetMask("Interactable");
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
                if (!obj.CheckNull() && respawnCollider == null)
                {
                    empty = false;
                } 
                else if(!obj.CheckNull() && respawnCollider != null)
                {
                    Collider[] colliders = Physics.OverlapBox(respawnCollider.center, (respawnCollider.size / 2f));
                    if (colliders.Contains(obj.ObjRef.GetComponent<Collider>()))
                    {
                        empty = false;
                    }
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
            }

            StartCoroutine(RunColliderRespawnCheck(empty));
        }
    }

    private IEnumerator RunColliderRespawnCheck(bool empty)
    {
        yield return new WaitForFixedUpdate();
        if (respawnCollider != null)
        {
            Vector3 rSize = new Vector3(Math.Abs(respawnCollider.size.x / 2f), Math.Abs(respawnCollider.size.y / 2f), Math.Abs(respawnCollider.size.z / 2f));
            Collider[] colliders = Physics.OverlapBox(transform.TransformPoint(respawnCollider.center), rSize, Quaternion.identity, mask);
            List<GameObject> objsUndetected = new List<GameObject>();

            foreach (RespawnableObject o in objects)
            {
                objsUndetected.Add(o.ObjRef);
            }
            foreach (Collider c in colliders)
            {
                if (objsUndetected.Contains(c.gameObject))
                {
                    objsUndetected.Remove(c.gameObject);
                }
            }

            foreach (GameObject o in objsUndetected)
            {
                foreach (RespawnableObject r in objects)
                {
                    if (r.ObjRef.Equals(o))
                    {
                        if (!waitForAll || empty)
                        {
                            r.RespawnObject(transform.position);
                        }
                    }
                }
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

    public void SetSpawnParent(GameObject spawnParent)
    {
        foreach (RespawnableObject obj in objects)
        {
            obj.SetSpawnParent(spawnParent);
            if(obj.ObjRef != null)
            {
                obj.ObjRef.transform.SetParent(spawnParent.transform, true);
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

    [SerializeField] public GameObject spawnParent;

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

        if (spawnParent != null)
            objRef.transform.SetParent(spawnParent.transform, true);
    }

    public void SetSpawnParent(GameObject parent)
    {
        this.spawnParent = parent;
    }
}