using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class FireEndingManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Instance")]
    public static FireEndingManager instance;

    [Header("Smoke Values")]
    public float smokiness;
    public float smokeRate;
    public float antiSmokeRate;
    public float fireEndingThreshold;

    private List<GameObject> fireObjects = new List<GameObject>();

    [Header("Prefabs")]
    public GameObject firePrefab;
    public GameObject smokeThingy;
    public GameObject redLight;

    [Header("Light Values")]
    public float lightTimer = 0;
    public bool lightEnabled = false;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent endingTrigger;

    [Header("References")]
    public GameManager gameManager;

    [Header("Events")]
    [SerializeField]VoidGameEvent endingEvent;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InvokeRepeating("removeNull", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float smokeValue = (antiSmokeRate + smokeRate * fireObjects.Count) * Time.deltaTime;
        if (smokeValue < 0)
        {
            smokeValue *= 4;
        }

        if (smokiness + smokeValue < 0)
        {
            smokeValue = 0f;
        }
        else if (smokiness >= fireEndingThreshold)
        {
            // Raise event to trigger achievement;
            endingEvent.RaiseEvent();

            if (gameManager != null)
            {
                gameManager.LoadGame(0);
            }
        }
        else if (smokiness >= fireEndingThreshold * .85)
        {
            smokiness += .3f * Time.deltaTime;
            if (lightTimer <= 0)
            {
                lightEnabled = !lightEnabled;
                lightTimer = .75f;
                AudioManager.instance.PlayOneShotSound(AudioManager.instance.fireAlarm);

            }
            if (redLight != null)
            {
                redLight.SetActive(lightEnabled);
            }

            lightTimer -= Time.deltaTime;
        }
        else
        {
            smokiness += smokeValue;
        }
        

        Color color;
        color = smokeThingy.GetComponent<Renderer>().material.color;
        //color.a = -Mathf.Pow(smokiness / fireEndingThreshold, 2) + 2 * (smokiness / fireEndingThreshold);
        float x = smokiness / fireEndingThreshold;
        color.a = 1 / (.5f * Mathf.Pow(x - 1, 2) - 1) + 2;
        smokeThingy.GetComponent<Renderer>().material.color = color;

        //if (smokiness > fireEndingThreshold)
        //{
        //    print("FIRE");
        //}
    }

    /// <summary>
    /// Removes all null objects from fireObjects
    /// </summary>
    private void removeNull()
    {
        fireObjects.RemoveAll(x => x == null);
    }

    /// <summary>
    /// Adds an object to the list of fire objects
    /// </summary>
    /// <param name="obj"></param>
    public void addFireObject(GameObject obj)
    {
        fireObjects.Add(obj);
    }

    /// <summary>
    /// Removes an object from the list of fire objects
    /// </summary>
    /// <param name="obj"></param>
    public void removeFireObject(GameObject obj)
    {
        fireObjects.Remove(obj);
    }
}
