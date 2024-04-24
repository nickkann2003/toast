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
    Tutorial,
    Objective,
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
                //DontDestroyOnLoad(gObj);
            }
            return instance;
        }
    }

    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    //public SceneLoadingManager SceneLoadingManager { get; private set; }

    public GameState curState = GameState.Menu;

    [SerializeField] Station gameDefaultStation;
    [SerializeField] Station tutorialStation;
    [SerializeField] Station mainMenuStation;

    // BGM
    public AudioClip test;
    public Raycast raycaster;

    // Cursor
    [SerializeField] private Texture2D cursorHand;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 DefaulthotSpot = new Vector2(16,16);
    [SerializeField] private Vector2 HandHotSpot = new Vector2(16, 0);

    [SerializeField] private Animator tutorialFinish;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        curState = GameState.Menu;
        raycaster.enabled = false;

        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = GetComponentInChildren<AudioManager>();
        UIManager = GameObject.Find("UIManager").gameObject.transform.GetComponent<UIManager>();
        //SceneLoadingManager = GetComponentInChildren<SceneLoadingManager>(); 
        //AudioManager.PlaySound(test);
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case GameState.Menu:
                break;
            case GameState.Tutorial:
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    PauseGame();
                }

                break;
            case GameState.inGame:
                if(Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    PauseGame();
                }

                // Check if the player is holding down the 'J' key
                if (Input.GetKey(KeyCode.J))
                {
                    // Enable the UI canvas if it's not already enabled
                    if (!UIManager.objectiveOpened)
                    {
                        UIManager.OpenObjectiveNote();
                    }
                }
                else
                {
                    // Disable the UI canvas if it's currently enabled
                    if (UIManager.objectiveOpened)
                    {
                        UIManager.CloseObjectiveNote();
                    }
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

    public void MainMenuToTutorial()
    {
        curState= GameState.Tutorial;

        Time.timeScale = 1;
        raycaster.enabled = true;
        UIManager.CloseMainMenu();
        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(tutorialStation);
    }

    public void TutorialToGame()
    {
        tutorialFinish.SetTrigger("FinishTutorial");
        StartCoroutine(WaitForTutorialAnimation());
    }

    private System.Collections.IEnumerator WaitForTutorialAnimation()
    {
        yield return new WaitForSeconds(2);

        // Once the animation is finished, execute the rest of your code
        curState = GameState.inGame;
        Time.timeScale = 1;
        raycaster.enabled = true;
        UIManager.CloseMainMenu();
        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(gameDefaultStation);
        UIManager.instance.SetupInGameUI();
    }

    public void PauseToMainMenu()
    {
       
        curState = GameState.Menu;
        Time.timeScale = 1;
        raycaster.enabled = false;

        UIManager.ClosePauseMenu();
        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(mainMenuStation);

        StartCoroutine(WaitForCameraMovement());
    }

    private IEnumerator WaitForCameraMovement()
    {
        // Wait until the camera movement is complete (you may need to adjust the condition based on your implementation)
        while (Vector3.Distance(Camera.main.transform.position, mainMenuStation.cameraPos) > 0.1)
        {
            yield return null; // Wait for the next frame
        }

        // Once the camera movement is complete, set the timescale to 0
        Time.timeScale = 0;
        UIManager.SetMainMain();
    }

    public void LoadGame(int sceneIndex)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
