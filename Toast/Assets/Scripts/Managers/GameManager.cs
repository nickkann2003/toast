using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


// Game state, used to track the game state
public enum GameState
{
    Menu,
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
    //public SceneLoadingManager SceneLoadingManager { get; private set; }

    public GameState curState = GameState.Menu;

    // BGM
    public AudioClip test;
    public Raycast raycaster;

    // Cursor
    [SerializeField] private Texture2D cursorHand;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 DefaulthotSpot = new Vector2(16,16);
    [SerializeField] private Vector2 HandHotSpot = new Vector2(16, 0);



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
  
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            curState = GameState.Menu;
        }
        else
        {
            curState = GameState.inGame;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = GetComponentInChildren<AudioManager>();
        UIManager = GameObject.Find("UIManager").gameObject.transform.GetComponent<UIManager>();
        //SceneLoadingManager = GetComponentInChildren<SceneLoadingManager>(); 
        AudioManager.PlaySound(test);
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case GameState.Menu:
                break;
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
        // Default cursor hot spot is 16, 16
        Cursor.SetCursor(null, DefaulthotSpot, Instance.cursorMode);
    }

    /// <summary>
    /// Set the hand cursor
    /// </summary>
    public void SetHandCursor()
    {
        Cursor.SetCursor(Instance.cursorHand, HandHotSpot, Instance.cursorMode);
    }

    public void PauseGame()
    {
        curState= GameState.Pause;
        Time.timeScale = 0;
        raycaster.enabled= false;
        UIManager.SetPauseMenu();
    }

    public void UnPauseGame()
    {
        curState = GameState.inGame;
        Time.timeScale = 1;
        raycaster.enabled = true;
        UIManager.ClosePauseMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeToGameScene()
    {
        curState = GameState.inGame;
        LoadGame(1);
        //UIManager = GameObject.Find("UIManager").gameObject.transform.GetComponent<UIManager>();
    }

    private void LoadGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
