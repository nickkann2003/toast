using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ------------------------------- Enums -------------------------------
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
    // ------------------------------- Instances -------------------------------
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

    // ------------------------------- Properties -------------------------------
    public AudioManager AudioManager { get; private set; }
    public UIManager UIManager { get; private set; }
    //public SceneLoadingManager SceneLoadingManager { get; private set; }

    // ------------------------------- Variables -------------------------------
    [Header("Current Game State")]
    public GameState curState = GameState.Menu;

    [Header("Station References")]
    [SerializeField] Station gameDefaultStation;
    [SerializeField] Station tutorialStation;
    [SerializeField] Station mainMenuStation;

    [Header("Music")]
    // BGM
    public AudioClip test;

    [Header("Raycaster")]
    public Raycast raycaster;

    [Header("Cursor References")]

    // Cursor
    [SerializeField] private Texture2D cursorHand;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 DefaulthotSpot = new Vector2(16,16);
    [SerializeField] private Vector2 HandHotSpot = new Vector2(16, 0);

    [Header("Animations")]
    [SerializeField] private Animator tutorialFinish;

    //  ------------------------------- Events (Test) -------------------------------
    //[Header("Events for Testing")]
    //[SerializeField] GameEvent eventTest; // Remove event raise at Line 220

    // ------------------------------- Functions -------------------------------
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
        // Switch for current game state
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

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        curState= GameState.Pause;
        Time.timeScale = 0;
        raycaster.enabled= false;
        UIManager.SetPauseMenu();
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnPauseGame()
    {
        curState = GameState.inGame;
        Time.timeScale = 1;
        raycaster.enabled = true;
        UIManager.ClosePauseMenu();
    }

    /// <summary>
    ///  Quits the game
    /// </summary>
    public void QuitGame()
    {
        SaveHandler.instance.SaveAllData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    /// <summary>
    /// Activates the main menu to tutorial animation
    /// </summary>
    public void MainMenuToTutorial()
    {
        curState= GameState.Tutorial;

        Time.timeScale = 1;
        raycaster.enabled = true;
        UIManager.CloseMainMenu();
        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(tutorialStation);
    }

    /// <summary>
    /// Activates the Tutorial to Game animation
    /// </summary>
    public void TutorialToGame()
    {
        tutorialFinish.SetTrigger("FinishTutorial");
        //eventTest.RaiseEvent();
        StartCoroutine(WaitForTutorialAnimation());
    }

    /// <summary>
    /// Waits for the tutorial animation to finish
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Activates the pause menu to main menu animation
    /// </summary>
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

    /// <summary>
    /// Waits for camera movement to complete
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Loads the game
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadGame(int sceneIndex)
    {
        SaveHandler.instance.SaveAllData();
        ObjectiveManager.instance.OnDisable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
