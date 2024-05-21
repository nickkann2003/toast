using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EventReceived(UnityEvent e)
    {
        switch(e)
        {
            default:
                break;
        }
    }

    /// <summary>
    /// Performs the steps of unlocking an achievement
    /// </summary>
    /// <param name="achievement">The achievement to unlock</param>
    private void Unlock(Achivement achievement)
    {
        if(!achievement.isUnlocked)
        {
            // Unlock code goes here
        }
    }
}
