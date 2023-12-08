using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingMenu;
    public UnityEngine.UI.Button backButton;
    public Animator backButtonAni;
    public Slider volumeSlider;
    public Slider moveSpeedSlider;
    private bool backButtonOnScreen = false;

    public bool backFromMainMenu = true;


    public static UIManager instance;

    private void Awake()
    {

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        //DontDestroyOnLoad(gameObject);
    }
  

    public void SetPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void SetMainMain()
    {
        mainMenu.SetActive(true);
    }

    public void CloseMainMenu() 
    {
        mainMenu.SetActive(false);
    }

    public void TurnOnBackButton()
    {
        if(!backButtonOnScreen)
        {
            backButton.interactable= true;
            backButtonAni.Play("BackButton_PopUp");
            backButtonOnScreen= true;
        }
    }

    public void TurnOffBackButton()
    {
        if(backButtonOnScreen)
        {
            backButton.interactable = false;
            //backButton.GetComponent<Button>().enabled = false;
            backButtonAni.Play("BackButton_PopDown");
            backButtonOnScreen= false;
        }
    }

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
}
