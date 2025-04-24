using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMainGame_AniEvent : MonoBehaviour
{
    private Animator ani;
    void Start()
    {
      ani = this.GetComponent<Animator>();  
    }

    /// <summary>
    /// This will be invoked in the animator controller as animation event
    /// </summary>
    public void MoveToMainGame(int fileId)
    {
        SaveHandler.instance.LoadSaveFile();
        SaveHandler.instance.SetCurrentSaveFileByID(fileId);
    }

    /// <summary>
    /// This will be invoked when the play button is clicked
    /// </summary>
    public void PlayBreadSlideInAnimation()
    {
        ani.SetTrigger("breadToToaster");
    }
}
