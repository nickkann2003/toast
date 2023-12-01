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
            if (obj != null)
            {
                ObjectVariables objVar = obj.GetComponent<ObjectVariables>();

                obj.GetComponent<Prop>().frozenness += Time.deltaTime;

                if (obj.GetComponent<Prop>().frozenness >= 5f && !objVar.attributes.Contains(Attribute.Frozen))
                {
                    Freeze(obj);
                    obj.GetComponent<Prop>().frozenness = 0.0f;
                }
            }
            else
            {
                collidingObjects.Remove(obj);
            }
        }
        //foreach (GameObject obj in collidingObjects)
        //{
        //    if (obj != null)
        //    {
        //        ObjectVariables objVar = obj.GetComponent<ObjectVariables>();

        //        obj.GetComponent<Prop>().frozenness += Time.deltaTime;

        //        if (obj.GetComponent<Prop>().frozenness >= 5f && !objVar.attributes.Contains(Attribute.Frozen))
        //        {
        //            Freeze(obj);
        //            obj.GetComponent<Prop>().frozenness = 0.0f;
        //        }
        //    }
        //    else
        //    {
        //        collidingObjects.Remove(obj);
        //    }
        //}
    }

    void Freeze(GameObject obj)
    {
        ObjectVariables objVar = obj.GetComponent<ObjectVariables>();

        if (objVar.attributes.Contains(Attribute.OnFire))
        {
            objVar.RemoveAttribute(Attribute.OnFire);
            Destroy(obj.transform.GetChild(0).gameObject);
        }
        else if (!objVar.attributes.Contains(Attribute.Frozen) && objVar.objectId == Object.Bread) 
        {
            GameObject ice = Instantiate(icePrefab);
            ice.transform.position = obj.transform.position;
            ice.transform.localScale = obj.GetComponent<Renderer>().bounds.size;
            ice.transform.parent = obj.transform;
            objVar.AddAttribute(Attribute.Frozen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if (!collidingObjects.Contains(other.gameObject) 
                && other.gameObject.GetComponent<Prop>() != null && other.gameObject.GetComponent<ObjectVariables>() != null)
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
                    other.gameObject.GetComponent<Prop>().frozenness = 0.0f;
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
