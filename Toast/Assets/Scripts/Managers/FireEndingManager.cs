using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireEndingManager : MonoBehaviour
{
    public static FireEndingManager instance;

    public float smokiness;
    public float smokeRate;
    public float antiSmokeRate;
    public float fireEndingThreshold;

    private List<GameObject> fireObjects = new List<GameObject>();

    public GameObject smokeThingy;

    [SerializeField] private UnityEvent endingTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        smokiness += (antiSmokeRate + smokeRate * fireObjects.Count) * Time.deltaTime;
        if (smokiness < 0)
        {
            smokiness = 0;
        }

        Color color;
        color = smokeThingy.GetComponent<Renderer>().material.color;
        color.a = (smokiness / fireEndingThreshold);
        smokeThingy.GetComponent<Renderer>().material.color = color;

        if (smokiness > fireEndingThreshold)
        {
            print("FIRE");
        }
    }

    public void addFireObject(GameObject obj)
    {
        fireObjects.Add(obj);
    }

    public void removeFireObject(GameObject obj)
    {
        fireObjects.Remove(obj);
    }
}
