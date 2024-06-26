using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------- Ice Prefab ------------")]
    public GameObject icePrefab;

    [SerializeField]
    private PropIntGameEvent freezeEvent;
    
    private List<GameObject> collidingObjects = new List<GameObject>();

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        if(freezeEvent == null)
        {
            freezeEvent = PieManager.instance.FreezeObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Loop all objects, set frozenness, freeze if over threshold
        for (int i = 0; i < collidingObjects.Count; i++)
        {
            GameObject obj = collidingObjects[i];
            if (obj != null && obj.GetComponent<Rigidbody>() != null)
            {
                NewProp prop = obj.GetComponent<NewProp>();

                prop.frozenness += Time.deltaTime;

                if (prop.frozenness >= 5f && !prop.propFlags.HasFlag(PropFlags.Frozen))
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

    // Freezes a given prop
    void Freeze(GameObject obj, NewProp prop)
    {
        // Put out fire if on fire, otherwise freeze
        if (prop.propFlags.HasFlag(PropFlags.OnFire))
        {
            freezeEvent.RaiseEvent(prop, 1);
            FireEndingManager.instance.removeFireObject(obj);
            prop.RemoveFlag(PropFlags.OnFire);
            Destroy(obj.transform.GetChild(0).gameObject);
        }
        else if (!prop.propFlags.HasFlag(PropFlags.Frozen) && prop.propFlags.HasFlag(PropFlags.Bread)) 
        {
            freezeEvent.RaiseEvent(prop, 1);
            GameObject ice = Instantiate(icePrefab);
            ice.transform.position = obj.transform.position;
            ice.GetComponent<MeshFilter>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
            ice.transform.parent = obj.transform;
            ice.transform.localEulerAngles = Vector3.zero;
            ice.transform.localScale = new Vector3(1.1f, 1.1f, 1.4f);

            prop.AddFlag(PropFlags.Frozen);
        }
    }

    // On enter trigger, add to freezing list
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

    // On trigger exist, remove from freezing list
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
