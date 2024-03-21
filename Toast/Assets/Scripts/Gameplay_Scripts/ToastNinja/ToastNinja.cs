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

    private ToastNinjaState toastNinjaState;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(timeRandMin, timeRandMax);
        toastNinjaState = ToastNinjaState.Active;
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
            yield return new WaitForSeconds(.25f);
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

    public void GameStart()
    {
        toastNinjaState = ToastNinjaState.Active;
    }

    public void GameReady()
    {
        toastNinjaObjects.active = true;
        toastNinjaState = ToastNinjaState.Ready;
    }
}
