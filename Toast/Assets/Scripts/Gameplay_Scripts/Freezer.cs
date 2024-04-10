using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();

    public GameObject icePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < collidingObjects.Count; i++)
        {
            GameObject obj = collidingObjects[i];
            if (obj != null && obj.GetComponent<Rigidbody>() != null)
            {
                NewProp prop = obj.GetComponent<NewProp>();

                prop.frozenness += Time.deltaTime;

                if (prop.frozenness >= 5f && !prop.attributes.HasFlag(PropFlags.Frozen))
                {
                    Freeze(obj, prop);
                    prop.frozenness = 0.0f;
                }
            }
            else
            {
                collidingObjects.Remove(obj);
            }
        }
    }

    void Freeze(GameObject obj, NewProp prop)
    {
        if (prop.attributes.HasFlag(PropFlags.OnFire))
        {
            FireEndingManager.instance.removeFireObject(obj);
            prop.RemoveAttribute(PropFlags.OnFire);
            Destroy(obj.transform.GetChild(0).gameObject);
        }
        else if (!prop.attributes.HasFlag(PropFlags.Frozen) && prop.attributes.HasFlag(PropFlags.Bread)) 
        {
            GameObject ice = Instantiate(icePrefab);
            ice.transform.position = obj.transform.position;
            ice.transform.localScale = obj.GetComponent<Renderer>().bounds.size;
            ice.transform.parent = obj.transform;
            prop.AddAttribute(PropFlags.Frozen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if (!collidingObjects.Contains(other.gameObject) 
                && other.gameObject.GetComponent<NewProp>() != null)
            {
                collidingObjects.Add(other.gameObject);
            }
        }
        catch
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            
            if (collidingObjects.Contains(other.gameObject))
            {
                //if (!other.gameObject.GetComponent<ObjectVariables>().attributes.Contains(Attribute.Frozen))
                //{
                //    other.gameObject.GetComponent<Prop>().frozenness = 0.0f;
                //}
                if (other != null)
                {
                    other.gameObject.GetComponent<NewProp>().frozenness = 0.0f;
                }
                collidingObjects.Remove(other.gameObject);
            }

        }
        catch
        {
            return;
        }
    }
}
