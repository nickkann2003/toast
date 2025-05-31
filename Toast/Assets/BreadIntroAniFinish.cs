using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadIntroAniFinish : MonoBehaviour
{
    public GameObject breadSlice;
    
    /// This will be called in the animation event
    public void OnAnimationIntroFinished()
    {
        breadSlice.SetActive(true);
        UIManager.instance.MoveToCloseMainMenu();
    }
}
