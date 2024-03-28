using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToastNinjaState
{
    Inactive,
    Ready,
    Active
}

public class ToastNinja : MonoBehaviour
{
    [SerializeField]
    private GameObject toastObj;
    [SerializeField]
    private GameObject jamObj;
    [SerializeField]
    private GameObject burningObj;

    [SerializeField]
    private float timeRandMin = .5f;
    [SerializeField]
    private float timeRandMax = 1f;

    private float timer;

    [SerializeField]
    private GameObject toastNinjaObjects;

    [SerializeField]
    private LaunchObject[] launchObjects;
    [SerializeField]
    private DestroyerVolume[] destroyerVolumes;

    private ToastNinjaState toastNinjaState;

    [SerializeField]
    private GameObject swordPrefab;
    private GameObject swordObject;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1;
        GameStop();
        //InvokeRepeating("LaunchToast", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch(toastNinjaState)
        {
            case ToastNinjaState.Active:
                timer -= Time.deltaTime;
                
                if (timer <= 0)
                {
                    StartCoroutine(RapidFire(5));
                    timer = Random.Range(timeRandMin, timeRandMax);
                }

                break;
        }
    }

    IEnumerator RapidFire(int amount)
    {
        if (amount > 0)
        {
            LaunchToast();
            yield return new WaitForSeconds(.4f);
            StartCoroutine(RapidFire(amount - 1));
        }
        else
        {
            yield return new WaitForSeconds(.1f);
        }
    }

    void LaunchToast()
    {
        float rand = Random.value;
        if (rand < .8)
        {
            launchObjects[Random.Range(0, launchObjects.Length)].Launch(toastObj);
        }
        else if(rand < .95)
        {
            launchObjects[Random.Range(0, launchObjects.Length)].Launch(jamObj);
        }
        else
        {
            launchObjects[Random.Range(0, launchObjects.Length)].Launch(burningObj);
        }
        //for (int i = 0; i < launchObjects.Length; i++)
        //{
        //    launchObjects[i].Use();
        //}
        //launchObjects[Random.Range(0, launchObjects.Length)].Launch();
    }

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

    public void GameStart()
    {
        if (Raycast.Instance.CheckKnifeStack() >= 3)
        {
            if (swordPrefab != null)
            {
                swordObject = Instantiate(swordPrefab);
                Sword swordScript = swordObject.GetComponent<Sword>();
                swordScript.hand.Pickup(Raycast.Instance.hand.CheckObject());
                Raycast.Instance.hand.Pickup(swordObject);

                Raycast.Instance.noDrop = true;
                Raycast.Instance.noDrag = true;
            }
        }

        toastNinjaState = ToastNinjaState.Active;
        //for (int i = 0; i < launchObjects.Length; i++)
        //{
        //    launchObjects[i].gameObject.SetActive(true);
        //}
        for (int i = 0;i < destroyerVolumes.Length; i++)
        {
            destroyerVolumes[i].gameObject.SetActive(true);
        }
    }

    public void GameStop()
    {
        if (swordObject != null)
        {
            Raycast.Instance.noDrop = true;
            Raycast.Instance.noDrag = true;

            Sword swordScript = swordObject.GetComponent<Sword>();

            Raycast.Instance.hand.Pickup(swordScript.hand.CheckObject());

            Destroy(swordObject);
            swordObject = null;
        }

        toastNinjaState = ToastNinjaState.Inactive;
        //for (int i = 0; i < launchObjects.Length; i++)
        //{
        //    launchObjects[i].gameObject.SetActive(false);
        //}
        for (int i = 0; i < destroyerVolumes.Length; i++)
        {
            destroyerVolumes[i].gameObject.SetActive(false);
        }
    }

    public void GameReady()
    {
        toastNinjaObjects.SetActive(true);
        toastNinjaState = ToastNinjaState.Ready;
    }
}
