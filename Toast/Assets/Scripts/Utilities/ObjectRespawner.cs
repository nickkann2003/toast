using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ObjectRespawner : MonoBehaviour
{
    public GameObject prefab;
    public int numItems;
    public Vector3 spawnPosition;
    public Vector3 perItemOffset = new Vector3(0.1f, 0.1f, 0.1f);
    private List<GameObject> objectRefs = new List<GameObject>();
    private bool reset = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numItems; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = spawnPosition + transform.position + perItemOffset * i;
            objectRefs.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If it was reset, respawn
        if (!objectRefs.Contains(null))
        {
            for (int i = 0; i < objectRefs.Count; i++)
            {
                if (objectRefs[i] == null)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.transform.position = spawnPosition + transform.position + perItemOffset * i;
                    objectRefs[i] = obj;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPosition + transform.position, 0.1f);
        for(int i = 0; i < numItems; i++)
        {
            Gizmos.DrawSphere(spawnPosition + transform.position + perItemOffset*i, 0.04f);
        }
    }
}
