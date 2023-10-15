using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// Game state, used to track the game state
public enum GameState
{
    Menu,
    inGame
}

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

    // BGM
    public AudioClip test;

    // Cursor
    public Texture2D cursorHand;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Outline


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
        //switch (curStation)
        //{
        //    case Stations.Starting:
        //        tableOutline.enabled = true;
        //        toasterOutline.enabled = false;
        //        dialOutline.enabled = false;

        //        backButton.SetActive(false);
        //        break;
        //    case Stations.Table:
        //        tableOutline.enabled = false;
        //        toasterOutline.enabled = true;
        //        dialOutline.enabled = false;

        //        backButton.SetActive(true);
        //        break;
        //    case Stations.Toaster:
        //        tableOutline.enabled = false;
        //        toasterOutline.enabled = false;
        //        dialOutline.enabled = true;
        //        backButton.SetActive(true);
        //        break;

        //}
    }

    /// <summary>
    /// Set the default cursor
    /// </summary>
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Instance.hotSpot, Instance.cursorMode);
    }

    /// <summary>
    /// Set the hand cursor
    /// </summary>
    public void SetHandCursor()
    {
        Cursor.SetCursor(Instance.cursorHand, Instance.hotSpot, Instance.cursorMode);
    }
}
