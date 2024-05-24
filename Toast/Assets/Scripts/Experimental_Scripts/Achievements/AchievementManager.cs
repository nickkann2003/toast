using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public List<Achievement> achievements;

    // Tester, remove later
    Achievement tester;

    StatBased statTest;

    // Basic singleton
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Test Achievement
        tester = new Achievement("Test", "Passed the test");
        statTest = new StatBased("Stat Test", "Got some stats", 5);
        achievements.Add(tester);
        achievements.Add(statTest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void EventReceived(GameEvent e)
    {
        switch(e)
        {
            default:
                Unlock(tester);
                break;
        }
    }
    */

    /// <summary>
    /// Performs the steps of unlocking an achievement
    /// </summary>
    /// <param name="achievement">The achievement to unlock</param>
    private void Unlock(Achievement achievement)
    {
        if(!achievement.IsUnlocked)
        {
            // Unlock code goes here
            achievement.IsUnlocked = true;
            Debug.Log("Achievement Unlocked: " + achievement.AchievementName + ": " + achievement.Description);
        }
    }

    public void AchievementTest()
    {
        Debug.Log("Testing Achievement!");
        Unlock(tester);
    }
}
