using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInfoManager : MonoBehaviour
{
    public GameObject UIPanel;

    public static UsingInfoManager instance;

    [SerializeField] NewHand playerHand;

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
}
