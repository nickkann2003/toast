using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class FireEndingManager : MonoBehaviour
{
    public static FireEndingManager instance;

    public float smokiness;
    public float smokeRate;
    public float antiSmokeRate;
    public float fireEndingThreshold;

    private List<GameObject> fireObjects = new List<GameObject>();

    public GameObject firePrefab;
    public GameObject smokeThingy;
    public GameObject redLight;
    public float lightTimer = 0;
    public bool lightEnabled = false;

    [SerializeField] private UnityEvent endingTrigger;

    public GameManager gameManager;

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

        if (smokiness + smokeValue < 0)
        {
            smokeValue = 0f;
        }
        else if (smokiness >= fireEndingThreshold)
        {
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
        color.a = (smokiness / fireEndingThreshold);
        smokeThingy.GetComponent<Renderer>().material.color = color;

        //if (smokiness > fireEndingThreshold)
        //{
        //    print("FIRE");
        //}
    }

    private void removeNull()
    {
        fireObjects.RemoveAll(x => x == null);
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
