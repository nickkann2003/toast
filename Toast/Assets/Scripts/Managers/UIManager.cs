using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private enum PauseScreen 
    { 
        none,
        pauseMenu,
        optionsMenu,
        achievements,
        fileSelect,
        fileName,
        credits
    }
    // ------------------------------- Variables -------------------------------
    [Header("Menu References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject objectiveNote;
    [SerializeField] private GameObject fileSelectMenu;
    [SerializeField] private GameObject saveFileNameMenu;
    [SerializeField] private GameObject achievementMenu;
    [SerializeField] private GameObject notificationBanner;
    [SerializeField] private GameObject creditsMenu;

    public bool objectiveOpened = false;
    private PauseScreen currentScreen = PauseScreen.none;

    [Header("UI Buttons")]
    public UnityEngine.UI.Button backButton;
    public Button objectiveButton;
    public Animator backButtonAni;
    public Slider volumeSlider;
    public Slider moveSpeedSlider;
    private bool backButtonOnScreen = false;
    
    public bool backFromMainMenu = true;

    public static UIManager instance;

    [Header("Physical File Selection")]

    [SerializeField] private Station fileSelectionStation;
    [SerializeField] private List<GameObject> saveSlots_Bread;
    [SerializeField] private List<GameObject> saveSlotButton_Blackframe;
    [SerializeField] private List<Station> saveSlotsStations;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //DontDestroyOnLoad(gameObject);
    }
    /// <summary>
    /// Handles escape presses when at the main menu
    /// </summary>
    public void MenuEscape()
    {
        if(currentScreen == PauseScreen.fileSelect)
        {
            CloseFileSelectMenu();
            SetMainMain();
        }
        if (currentScreen == PauseScreen.fileName)
        {
            CloseFileNamingMenu();
            OpenFileSelectMenu();
        }
        if(currentScreen == PauseScreen.optionsMenu)
            CloseSettingMenuFromMainMenu();
    }

    /// <summary>
    /// Sets pause menu active
    /// </summary>
    public void SetPauseMenu()
    {
        pauseMenu.SetActive(true);
        float timeDelay = 0.0f;
        foreach (Animator a in pauseMenu.GetComponentsInChildren<Animator>())
        {
            StartCoroutine(AnimateButtons(timeDelay, a, "Open"));
            timeDelay += 0.05f;
        }
        currentScreen = PauseScreen.pauseMenu;
    }

    /// <summary>
    /// Sets pause menu false, returns true if pause was closed, false if pause was moved back one state
    /// </summary>
    public bool ClosePauseMenu()
    {
        bool closed = false;
        if(currentScreen == PauseScreen.pauseMenu)
        {
            pauseMenu.SetActive(false);
            settingMenu.SetActive(false);
            foreach (Animator a in pauseMenu.GetComponentsInChildren<Animator>())
            {
                StartCoroutine(AnimateButtons(0f, a, "Close"));
            }
            closed = true;
        }
        else
        {
            if (currentScreen == PauseScreen.achievements)
                CloseAchievementMenu();
            settingMenu.SetActive(false);

            SetPauseMenu();
        }

        return closed;
    }

    private IEnumerator AnimateButtons(float timeDelay, Animator a, string trigger)
    {
        yield return new WaitForSecondsRealtime(timeDelay);
        a.SetTrigger(trigger);
    }

    /// <summary>
    /// Sets main menu true
    /// </summary>
    public void SetMainMain()
    {
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Sets main menu false
    /// </summary>
    public void CloseMainMenu() 
    {
        mainMenu.SetActive(false);
    }

    /// <summary>
    /// Sets up the in game UI
    /// </summary>
    public void SetupInGameUI()
    {
        backButton.gameObject.SetActive(true);
        objectiveButton.gameObject.SetActive(true);
    }

    public void BackButtonPopup()
    {
        if(!backButtonOnScreen)
        {
            backButton.interactable= true;
            backButtonAni.Play("BackButton_PopUp");
            backButtonOnScreen= true;
        }
    }

    /// <summary>
    /// Starts the back button exit screen animation
    /// </summary>
    public void BackButtonPopdown()
    {
        if(backButtonOnScreen)
        {
            backButton.interactable = false;
            //backButton.GetComponent<Button>().enabled = false;
            backButtonAni.Play("BackButton_PopDown");
            backButtonOnScreen= false;
        }
    }

    /// <summary>
    /// Opens objective notes
    /// </summary>
    public void OpenObjectiveNote()
    {
        objectiveOpened = true;
        objectiveNote.SetActive(true);

    }

    /// <summary>
    /// Closes objective notes
    /// </summary>
    public void CloseObjectiveNote()
    {
        objectiveOpened = false;
        objectiveNote.SetActive(false);    
    }

    /// <summary>
    /// Opens the settings menu
    /// </summary>
    public void OpenSettingMenu()
    {
        if(mainMenu.activeSelf)
        {
            backFromMainMenu = true;
        }
        else
        {
            backFromMainMenu = false;
        }
        settingMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        currentScreen = PauseScreen.optionsMenu;
    }

    /// <summary>
    /// Closes the settings menu
    /// </summary>
    public void CloseSettingMenuFromMainMenu()
    {
        settingMenu.SetActive(false);
        if (backFromMainMenu)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
        currentScreen = PauseScreen.none;
    }

    /// <summary>
    /// Opens the credits menu
    /// </summary>
    public void OpenCreditsMenu()
    {
        if (mainMenu.activeSelf)
        {
            backFromMainMenu = true;
        }
        else
        {
            backFromMainMenu = false;
        }
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        currentScreen = PauseScreen.credits;
    }

    /// <summary>
    /// Closes the settings menu
    /// </summary>
    public void CloseCreditsFromMainMenu()
    {
        creditsMenu.SetActive(false);
        if (backFromMainMenu)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
        currentScreen = PauseScreen.none;
    }

#region File Selection
    /// <summary>
    /// Opens the file select menu
    /// </summary>
    public void OpenFileSelectMenu()
    {
        //fileSelectMenu.SetActive(true);
        currentScreen = PauseScreen.fileSelect;

        // Move to the physical file selection stations
        StationManager.instance.MoveToStation(fileSelectionStation);
    }

    /// <summary>
    /// Closes the file select menu
    /// </summary>
    public void CloseFileSelectMenu()
    {
        fileSelectMenu?.SetActive(false);
    }

    /// <summary>
    /// Opens the file naming menu
    /// </summary>
    public void OpenFileNamingMenu()
    {
        saveFileNameMenu.SetActive(true);
        currentScreen = PauseScreen.fileName;
    }

    /// <summary>
    /// Closes the file naming menu
    /// </summary>
    public void CloseFileNamingMenu()
    {
        saveFileNameMenu?.SetActive(false);
    }

    /// <summary>
    /// Will be called when the saveslot black frame is clicked
    /// </summary>
    /// <param name="fileIndex">Which slot is clicked</param>
    public void AddNewSaveFile(int fileIndex)
    {
        saveSlotButton_Blackframe[fileIndex].SetActive(false);
        saveSlots_Bread[fileIndex].SetActive(true);
    }

    public void MoveToFileSlotStation(int fileIndex)
    {
        StationManager.instance.MoveToStation(saveSlotsStations[fileIndex]);
    }

    public void BackToMainFileSelectStation()
    {
        // Move to the physical file selection stations
        StationManager.instance.MoveToStation(fileSelectionStation);
    }

#endregion

#region Achievement

    /// <summary>
    /// Opens the achievement menu
    /// </summary>
    public void SetAchievementMenu()
    {
        // Pause game (in case menu is accessed via banner click)
        GameManager.Instance.PauseGame();

        // Show achievement menu
        achievementMenu.SetActive(true);

        PieManager.instance.ViewAchievements.RaiseEvent(new NewProp(), 1);

        // Hide pause menu
        pauseMenu.SetActive(false);

        currentScreen = PauseScreen.achievements;
    }

    /// <summary>
    /// Closes the achievement menu
    /// </summary>
    public void CloseAchievementMenu()
    {
        achievementMenu?.SetActive(false);
        PieManager.instance.ViewAchievements.RaiseEvent(new NewProp(), 1);
        SetPauseMenu();
        currentScreen = PauseScreen.pauseMenu;
    }
#endregion
}
