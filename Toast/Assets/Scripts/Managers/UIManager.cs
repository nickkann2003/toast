using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Menu References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private GameObject objectiveNote;
    [SerializeField] private GameObject fileSelectMenu;
    [SerializeField] private GameObject saveFileNameMenu;
    public bool objectiveOpened = false;

    [Header("UI Buttons")]
    public UnityEngine.UI.Button backButton;
    public UnityEngine.UI.Button inventoryButton;
    public Animator backButtonAni;
    public Slider volumeSlider;
    public Slider moveSpeedSlider;
    private bool backButtonOnScreen = false;
    private bool inventoryButtonOnScreen = false;
    
    public bool backFromMainMenu = true;

    public static UIManager instance;

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
    /// Sets pause menu active
    /// </summary>
    public void SetPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Sets pause menu false
    /// </summary>
    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);
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
        inventoryButton.gameObject.SetActive(true);
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
    /// Activates the inventory button
    /// </summary>
    public void TurnOnInventoryButton()
    {
        if (!inventoryButtonOnScreen)
        {
            inventoryButton.interactable = true;
            inventoryButtonOnScreen = true;
        }
    }

    /// <summary>
    /// Deactivates the inventory button
    /// </summary>
    public void TurnOffInventoryButton()
    {
        inventoryButton.interactable = false;
        inventoryButtonOnScreen = false;
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
 
    }

    /// <summary>
    /// Opens the file select menu
    /// </summary>
    public void OpenFileSelectMenu()
    {
        fileSelectMenu.SetActive(true);
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
    }

    /// <summary>
    /// Closes the file naming menu
    /// </summary>
    public void CloseFileNamingMenu()
    {
        saveFileNameMenu?.SetActive(false);
    }
}
