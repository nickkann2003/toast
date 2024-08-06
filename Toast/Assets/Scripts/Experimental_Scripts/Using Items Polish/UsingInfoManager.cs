using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInfoManager : MonoBehaviour
{
    public GameObject UIPanel;

    public static UsingInfoManager instance;

    [SerializeField] NewHand playerHand;

    public ToastNinja toastNinja;

    [SerializeField] GameObject useTextUI;

    // Singleton
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInfo(NewProp prop, int num)
    {
        UIPanel.transform.position = Camera.main.WorldToScreenPoint(prop.transform.position); 
        UIPanel.SetActive(true);

        // Display use if use effect exists
        if(prop.UseEffectsCount > 0)
        {
            useTextUI.SetActive(true);
        }
        else
        {
            useTextUI.SetActive(false);
        }

        // Add more conditions for special cases like knife and spread which can only be used in hand
    }

    public void HideInfo(NewProp prop, int num)
    {
        UIPanel.SetActive(false);
    }
}
