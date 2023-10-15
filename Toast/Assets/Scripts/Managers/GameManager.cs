using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get 
        {
            if (instance == null)
            {
                GameObject gObj = new GameObject();
                gObj.name = "GameManager";
                instance = gObj.AddComponent<GameManager>();
                DontDestroyOnLoad(gObj);
            }
            return instance;
        }
    }

    public AudioManager AudioManager { get; private set; }

    public AudioClip test;

    public Texture2D cursorHand;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        AudioManager = GetComponentInChildren<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.PlaySound(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
