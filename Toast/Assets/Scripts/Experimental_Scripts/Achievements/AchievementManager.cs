using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    // The full list of achievements in the game
    public List<Achievement> achievements;

    // The list of acheievements the player has unlocked
    public List<Achievement> unlockedAchievements;

    // Testers, remove later
    Achievement tester;
    Achievement statTest;

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
        statTest = new Achievement("Stat Test", "Got some stats", 5);
        achievements.Add(tester);
        achievements.Add(statTest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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
            unlockedAchievements.Add(achievement);
        }
    }

    public void AchievementTest()
    {
        Debug.Log("Testing Achievement!");
        Unlock(tester);
    }
}
