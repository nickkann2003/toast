using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    public UnityEngine.UI.Button backButton;
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
}
