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
                    LaunchToast();
                    timer = Random.Range(timeRandMin, timeRandMax);
                }

                break;
        }
    }

    void LaunchToast()
    {
        //for (int i = 0; i < launchObjects.Length; i++)
        //{
        //    launchObjects[i].Use();
        //}
        launchObjects[Random.Range(0, launchObjects.Length)].Launch();
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
