using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------- Enums -------------------------------
public enum ToastNinjaState
{
    Inactive,
    Ready,
    Active
}

public class ToastNinja : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Prefab References")]
    [SerializeField]
    private GameObject toastObj;
    [SerializeField]
    private GameObject jamObj;
    [SerializeField]
    private GameObject burningObj;

    [SerializeField]
    private GameObject swordObject;

    [Header("Time Variables")]
    [SerializeField]
    private float timeRandMin = .5f;
    [SerializeField]
    private float timeRandMax = 1f;

    private float timer;

    [Header("Current Arrays")]
    [SerializeField]
    private GameObject toastNinjaObjects;
    [SerializeField]
    private LaunchObject[] launchObjects;
    [SerializeField]
    private TN_FiringPatterns[] firingPatterns;

    [Header("Destroyer Volumes")]
    [SerializeField]
    private DestroyerVolume[] destroyerVolumes;

    [SerializeField, ReadOnly]
    private ToastNinjaState toastNinjaState;

    [SerializeField]
    private List<GameObject> moveBlockers;

    [SerializeField]
    private RS_ToastNinja runtimeSet;

    [SerializeField]
    private ToastNinjaScore scoreSO;

    [SerializeField]
    private GameObject toastNinjaUI;

    // ------------------------------- Properties -------------------------------
    public LaunchObject[] LaunchObjects
    {
        get { return launchObjects; }
    }

    public ToastNinjaState CurrentState
    {
        get { return toastNinjaState; }
    }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        GameReset();
        //InvokeRepeating("LaunchToast", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Switch for game state
        switch(toastNinjaState)
        {
            // Active, run toast ninja
            case ToastNinjaState.Active:
                timer -= Time.deltaTime;
                
                if (timer <= 0)
                {
                    //firingPatterns[0].Launch(this);
                    firingPatterns[Random.Range(0, firingPatterns.Length - 1)].Launch(this);
                    timer = Random.Range(timeRandMin, timeRandMax);
                }

                break;
        }
    }

    // Randomly creates a wave of items
    void LaunchToast()
    {
        launchObjects[Random.Range(0, launchObjects.Length)].Launch(RandPrefab());

        //for (int i = 0; i < launchObjects.Length; i++)
        //{
        //    launchObjects[i].Use();
        //}
        //launchObjects[Random.Range(0, launchObjects.Length)].Launch();
    }

    public GameObject RandPrefab()
    {
        float rand = Random.value;
        if (rand < .8)
        {
            return toastObj;
        }
        else if (rand < .95)
        {
            return jamObj;
        }
        else
        {
            return burningObj;
        }
    }

    // Toggles toast ninja active state
    public void ToggleActive()
    {
        if (toastNinjaState == ToastNinjaState.Active)
        {
            GameStop();
        }
        else
        {
            GameStart();
        }
    }

    // Starts Toast Ninja
    public void GameStart()
    {
        //if (Raycast.Instance.CheckKnifeStack() >= 3)
        //{
        //    if (swordPrefab != null)
        //    {
        //        swordObject = Instantiate(swordPrefab);
        //        Sword swordScript = swordObject.GetComponent<Sword>();
        //        swordScript.hand.Pickup(Raycast.Instance.hand.CheckObject());
        //        Raycast.Instance.hand.Pickup(swordObject);

        //        Raycast.Instance.noDrop = true;
        //        Raycast.Instance.noDrag = true;
        //    }
        //}

        if (Raycast.Instance.hand.CheckObject() != swordObject)
        {
            Raycast.Instance.hand.Drop().transform.position = StationManager.instance.playerLocation.ObjectOffset;
            Raycast.Instance.hand.Pickup(swordObject);
        }

        Raycast.Instance.noDrag = true;

        toastNinjaState = ToastNinjaState.Active;
        for (int i = 0; i < launchObjects.Length; i++)
        {
            launchObjects[i].active = true;
        }
        for (int i = 0;i < destroyerVolumes.Length; i++)
        {
            destroyerVolumes[i].gameObject.SetActive(true);
        }
        foreach(GameObject blocker in moveBlockers)
        {
            blocker.SetActive(true);
        }

        toastNinjaUI.SetActive(true);
    }

    // Stops Toast Ninja
    public void GameStop()
    {
        //if (swordObject != null)
        //{
        //    Raycast.Instance.noDrop = false;
        //    Raycast.Instance.noDrag = false;

        //    Sword swordScript = swordObject.GetComponent<Sword>();

        //    Raycast.Instance.hand.Pickup(swordScript.hand.CheckObject());

        //    Destroy(swordObject);
        //    swordObject = null;
        //}

        Raycast.Instance.noDrag = false;

        toastNinjaState = ToastNinjaState.Inactive;
        for (int i = 0; i < launchObjects.Length; i++)
        {
            launchObjects[i].active = false;
        }
        for (int i = 0; i < destroyerVolumes.Length; i++)
        {
            destroyerVolumes[i].gameObject.SetActive(false);
        }
        foreach (GameObject blocker in moveBlockers)
        {
            blocker.SetActive(false);
        }

        scoreSO.GameEnd();

        runtimeSet.DestroyAll();

        toastNinjaUI.SetActive(false);
    }

    private void GameReset()
    {
        Raycast.Instance.noDrag = false;

        toastNinjaState = ToastNinjaState.Inactive;
        for (int i = 0; i < launchObjects.Length; i++)
        {
            launchObjects[i].active = false;
        }
        for (int i = 0; i < destroyerVolumes.Length; i++)
        {
            destroyerVolumes[i].gameObject.SetActive(false);
        }
        foreach (GameObject blocker in moveBlockers)
        {
            blocker.SetActive(false);
        }

        scoreSO.ResetScore();

        runtimeSet.DestroyAll();

    }

    // Readies the game
    public void GameReady()
    {
        toastNinjaObjects.SetActive(true);
        toastNinjaState = ToastNinjaState.Ready;
    }
}
