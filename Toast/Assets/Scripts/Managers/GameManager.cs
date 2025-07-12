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
    inGame,
    Intro
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

    public GameObject PauseMenu;
    //public SceneLoadingManager SceneLoadingManager { get; private set; }

    // ------------------------------- Variables -------------------------------
    [Header("Current Game State")]
    public GameState curState = GameState.Menu;

    [Header("Station References")]
    [SerializeField] Station gameDefaultStation;
    [SerializeField] Station tutorialStation;
    [SerializeField] Station mainMenuStation;

    [Header("Intro References")]
    public Animator introAnimation;
    public Animator introBreadAnimation;
    public float introTime;
    private float rIntroTime;

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
        // Lock cursor
        Cursor.lockState = CursorLockMode.Confined;

        #if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
        #endif

        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        curState = GameState.Intro;
        rIntroTime = introTime;
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

        introAnimation.SetTrigger("Intro");
        introBreadAnimation.SetTrigger("Intro");



        // Move to the physical stations
        StationManager.instance.MoveToStation(mainMenuStation);
    }

    // Update is called once per frame
    void Update()
    {
        // Switch for current game state
        switch(curState)
        {
            case GameState.Intro:
                rIntroTime -= Time.deltaTime;
                if(rIntroTime < 0)
                {
                    curState = GameState.Menu;
                }
                break;
            case GameState.Menu:
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    UIManager.MenuEscape();
                }
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

                //// Check if the player is holding down the 'J' key
                //if (Input.GetKey(KeyCode.J))
                //{
                //    // Enable the UI canvas if it's not already enabled
                //    if (!UIManager.objectiveOpened)
                //    {
                //        UIManager.OpenObjectiveNote();
                //    }
                //}
                //else
                //{
                //    // Disable the UI canvas if it's currently enabled
                //    if (UIManager.objectiveOpened)
                //    {
                //        UIManager.CloseObjectiveNote();
                //    }
                //}
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
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        raycaster.enabled= false;
        UIManager.SetPauseMenu();
    }

    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnPauseGame()
    {
        bool unpaused = UIManager.ClosePauseMenu();
        if (unpaused)
        {
            curState = GameState.inGame;
            // Confine cursor
            Cursor.lockState = CursorLockMode.Confined;
            #if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.None;
            #endif
            Time.timeScale = 1;
            raycaster.enabled = true;
            UIManager.ClosePauseMenu();
        }
    }

    /// <summary>
    ///  Quits the game
    /// </summary>
    public void QuitGame()
    {
        StartCoroutine(QuitCoroutine());
    }

    private IEnumerator QuitCoroutine()
    {
        try
        {
            SaveHandler.instance.SaveAllData();
        }
        catch
        {
            Debug.LogError("Save failed! This could lead to lost or corrupt data potentially.");
        }

        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
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
        MusicManager.instance.SetMusicClip(2);
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
        Time.timeScale = 0f;
        UIManager.SetMainMain();
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadGame(int sceneIndex)
    {
        try
        {
            SaveHandler.instance.SaveAllData();
            ObjectiveManager.instance.OnDisable();
        }
        catch
        {
            // another catch block with node code! Get rolled! haha! (i really need to actually fix these issues but this is fine for ROC game fest)
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
