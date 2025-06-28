using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryVolume : MonoBehaviour
{
    [SerializeField]
    Carrier carrierObject;


    public List<GameObject> currentCarries;

    Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        currentCarries = new List<GameObject>();
        if (carrierObject == null)
        {
            carrierObject = gameObject.GetComponentInParent<Carrier>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't pick up hands
        if(other.gameObject.GetComponent<NewHand>())
        {
            return; 
        }

        if(!carrierObject.isHeld)
        {
            // Only include interactable objects that aren't the parent
            if (other.GetComponent<NewProp>())
            {
                currentCarries.Add(FindParent(other.gameObject));
                FindParent(other.gameObject).transform.SetParent(carrierObject.gameObject.transform);

                /*
                if(other.transform.parent!=null)
                {
                    if(!currentCarries.Contains(other.transform.parent.gameObject))
                    {
                        currentCarries.Add(other.transform.parent.gameObject);
                        other.transform.parent.SetParent(carrierObject.gameObject.transform);
                    }
                }    
                else
                {
                    if(!currentCarries.Contains(other.gameObject))
                    {
                        currentCarries.Add(other.gameObject);
                        other.transform.SetParent(carrierObject.gameObject.transform);
                    }
                }
                */
                
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!carrierObject.isHeld && currentCarries.Contains(other.gameObject))
        {
            currentCarries.Remove(FindParent(other.gameObject));
            FindParent(other.gameObject).transform.parent = null;

           /*
           currentCarries.Remove(other.gameObject);
           other.transform.parent = null;
           */
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Used to find the top level parent object of an object
    /// </summary>
    /// <param name="obj">The object to check</param>
    /// <returns>The highest parent of the original object</returns>
    GameObject FindParent(GameObject obj)
    {
        // Parent is carrier, don't want it so return as top level
        if(obj.transform.parent == carrierObject.gameObject.transform)
        {
            return obj;
        }

        // Has a parent
        if(obj.transform.parent != null)
        {
            // Recursion, check this parent's parent
            return FindParent(obj.transform.parent.gameObject);
        }
        // No parent, top level found
        else
        {
            return obj;
        }

        
    }
}
