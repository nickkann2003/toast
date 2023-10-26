using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


// Game state, used to track the game state
public enum GameState
{
    Pause,
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
    public UIManager UIManager { get; private set; }

    public GameState curState;

    // BGM
    public AudioClip test;

    // Cursor
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
        UIManager= GetComponentInChildren<UIManager>();

        curState = GameState.inGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.PlaySound(test);
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case GameState.inGame:
                if(Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    PauseGame();
                }
                break;
            case GameState.Pause:
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    UnPauseGame();
                }
                break;
        }  
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

    public void PauseGame()
    {
        curState= GameState.Pause;
        Time.timeScale = 0;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        UIManager.SetPauseMenu();
    }

    public void UnPauseGame()
    {
        curState = GameState.inGame;
        Time.timeScale = 1;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        UIManager.ClosePauseMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
