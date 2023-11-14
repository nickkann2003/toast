using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    public GameObject backButton;
    public Animator backButtonAni;
    private bool backButtonOnScreen = false;

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
            backButtonAni.Play("BackButton_PopUp");
            backButtonOnScreen= true;
        }
        //backButton.SetActive(true);
    }

    public void TurnOffBackButton()
    {
        if(backButtonOnScreen)
        {
            backButtonAni.Play("BackButton_PopDown");
            backButtonOnScreen= false;
        }
        //backButton.SetActive(false);
    }
}
