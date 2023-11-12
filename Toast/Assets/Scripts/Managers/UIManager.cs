using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;

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

    public void SetMainMenu()
    {
        mainMenu.SetActive(true);
    }

    public void CloseMainMenu()
    {
        mainMenu.SetActive(false);
    }
}
