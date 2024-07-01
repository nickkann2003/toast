using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    [SerializeField]
    private StatType freezeType;

    // ------------------------------- Variables -------------------------------
    [Header("------------- Ice Prefab ------------")]
    public GameObject icePrefab;

    [SerializeField]
    private FrozenAttribute frozenAttribute;

    [SerializeField]
    private PropIntGameEvent freezeEvent;

    [SerializeField]
    private List<NewProp> collidingObjects = new List<NewProp>();

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
            NewProp prop = collidingObjects[i];
            if (prop != null)
            {
                prop.Stats.IncrementStat(freezeType, Time.deltaTime);
                prop.Stats.GetStat(freezeType).UpdateValue();

                //prop.frozenness += Time.deltaTime;

                //if (prop.frozenness >= 5f)
                //{
                //    Freeze(prop);
                //    prop.frozenness = 0.0f;
                //}

            }
            else
            {
                collidingObjects.Remove(prop);
            }
        }
    }

    // Freezes a given prop
    void Freeze(NewProp prop)
    {
        prop.AddAttribute(frozenAttribute);
        //// Put out fire if on fire, otherwise freeze
        //if (prop.propFlags.HasFlag(PropFlags.OnFire))
        //{
        //    freezeEvent.RaiseEvent(prop, 1);
        //    FireEndingManager.instance.removeFireObject(prop.gameObject);
        //    prop.RemoveFlag(PropFlags.OnFire);
        //    Destroy(prop.transform.GetChild(0).gameObject);
        //}
        //else if (!prop.HasAttribute(frozenAttribute)) 
        //{
        //    freezeEvent?.RaiseEvent(prop, 1);
        //    prop.AddAttribute(frozenAttribute);
        //}
    }

    // On enter trigger, add to freezing list
    void OnTriggerEnter(Collider other)
    {
        try
        {
            NewProp prop = other.gameObject.GetComponent<NewProp>();

            if (prop != null &&
                !collidingObjects.Contains(prop))
            {
                collidingObjects.Add(prop);
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
        Debug.Log("HELP");
        try
        {
            NewProp prop = other.gameObject.GetComponent<NewProp>();
            if (prop != null &&
                collidingObjects.Contains(prop))
            {
                collidingObjects.Remove(prop);
                prop.frozenness = 0.0f;
            }

        }
        catch
        {
            return;
        }
    }
}
