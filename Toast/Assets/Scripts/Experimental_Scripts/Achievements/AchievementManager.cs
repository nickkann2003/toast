using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    // DECLARE ACHIEVEMENTS============================================
    [Foldout("Achievement Objects")]
    public Achievement ACHIEVEMENT_CLEAR_TUTORIAL;
    [Foldout("Achievement Objects")]
    public Achievement ACHIEVEMENT_EAT_5_BREAD;





    // MAIN MANAGER====================================================
    public static AchievementManager instance;

    // The full list of achievements in the game
    public List<Achievement> achievements;

    // The list of acheievements the player has unlocked
    public List<Achievement> unlockedAchievements;

    [Header("Achievement Menu UI Elements")]
    // Prefab for achievement menu UI
    [SerializeField]
    private GameObject UIPrefab;

    // The panel containing the achievement list
    [SerializeField]
    private GameObject menuPanel;

    [Header("Unlock Notification Objects")]
    [SerializeField]
    private GameObject notificationBanner;
    [SerializeField]
    TextMeshProUGUI bannerText;

    //Temporary before animation, remove later********
    int bannerShowTime;

    // Basic singleton
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMenu();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(notificationBanner.activeSelf == true && bannerShowTime >= 3)
        {
            notificationBanner.SetActive(false);
        }
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
            PlayNotification(achievement);
        }
    }

    /// <summary>
    /// Unlocks an achievement using ID
    /// </summary>
    /// <param name="ID">The ID of the requested achievement</param>
    private void UnlockByID(int ID)
    {
        Unlock(achievements[ID]);
    }

    /// <summary>
    /// Achievements Relating to the tutorial
    /// </summary>
    public void TutorialCleared()
    {
        Unlock(ACHIEVEMENT_CLEAR_TUTORIAL);
    }

    /// <summary>
    /// Achievements Relating to eating
    /// </summary>
    public void ReceivedEat()
    {
        // Needs a way to check what is being eaten
        IncrementAchievement(ACHIEVEMENT_EAT_5_BREAD);
    }

    void IncrementAchievement(Achievement achievement)
    {
        // Check that achievement has goal and that it is greater than 0
        if(achievement.HasNumericGoal && achievement.AchievementGoal > 0)
        {
            // Increase progress
            achievement.AchievementProgress++;

            // If goal has been reached, unlock
            if(achievement.AchievementProgress >= achievement.AchievementGoal)
            {
                Unlock(achievement);
            }
        }
    }

    void CreateMenu()
    {
        for(int i = 0; i < achievements.Count; i++)
        {
            GameObject newItem = Instantiate(UIPrefab);
            newItem.GetComponent<AchievementDisplay>().associatedAchievement = achievements[i];
            newItem.transform.SetParent(menuPanel.transform, false);
        }
    }

    void PlayNotification(Achievement achievement)
    {
        bannerText.text = $"Achievement Unlocked: {achievement.AchievementName}: {achievement.Description}";
        // Play a sound here
        notificationBanner.SetActive(true);
        bannerShowTime = 0;
    }
}
