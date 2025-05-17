using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    // DECLARE ACHIEVEMENTS============================================
    [Foldout("Achievement Objects")]
    public Achievement
        ACHIEVEMENT_CLEAR_TUTORIAL,
        ACHIEVEMENT_EAT_5_BREAD,
        ACHIEVEMENT_FIRE_ENDING,
        ACHIEVEMENT_FIRST_TOAST,
        ACHIEVEMENT_FEED_LITTLE_FELLA,
        ACHIEVEMENT_TOAST_NINJA_SCORE_50,
        ACHIEVEMENT_TOAST_NINJA_SCORE_100,
        ACHIEVEMENT_ELECTROCUTION,
        ACHIEVEMENT_USE_50_JAM,
        ACHIEVEMENT_USE_CLOCK,
        ACHIEVEMENT_BUTTER_BREAD,
        ACHIEVEMENT_AVOCADO_TOAST;







    // MAIN MANAGER====================================================
    public static AchievementManager instance;

    // The full list of achievements in the game
    public List<Achievement> achievements;
    private Dictionary<int, Achievement> achievementsById = new Dictionary<int, Achievement>(); // List of achievements sorted by ID

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
    private Animator bannerAnimator;
    [SerializeField]
    private Animator spinningBreadAnimator;
    [SerializeField]
    private Animator newAchievementPopup;
    [SerializeField]
    TextMeshProUGUI bannerText;

    [Header("Achievement Progress Text")]
    [SerializeField]
    TextMeshProUGUI unlockedProgressText;
    [SerializeField]
    List<Sprite> progressionToastSprites;
    [SerializeField]
    Image progressionToastImage;

    [Header("Audio")]
    [SerializeField]
    private SimpleAudioEvent achievementUnlockEvent;

    [Header("Achievement Tracking")]
    public bool TrackingAchievements = true;



    // ------------------------------- Save Variables -------------------------------
    private string achievementSeparator = "~";
    private string spacer = "_";


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

    /// <summary>
    /// Performs the steps of unlocking an achievement
    /// </summary>
    /// <param name="achievement">The achievement to unlock</param>
    private void Unlock(Achievement achievement)
    {
        // Not tracking achievements, don't do anything
        if(!TrackingAchievements)
        {
            return; 
        }

        if(!achievement.IsUnlocked)
        {
            // Unlock code goes here
            achievement.IsUnlocked = true;
            Debug.Log("Achievement Unlocked: " + achievement.AchievementName + ": " + achievement.Description);
            unlockedAchievements.Add(achievement);
            achievement.MenuSquare.displayImage.sprite = achievement.MenuSquare.unlockedSprite;
            PlayNotification(achievement);
            UpdateProgressionToast();
            AudioManager.instance.PlayAudioEvent(achievementUnlockEvent);
        }
    }

    private void UpdateProgressionToast()
    {
        unlockedProgressText.text = $"{unlockedAchievements.Count}/{achievements.Count}";
        int index = Mathf.RoundToInt((float)unlockedAchievements.Count / achievements.Count * progressionToastSprites.Count);
        progressionToastImage.sprite = progressionToastSprites[index];
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
    public void ReceivedEat(NewProp prop, int increment)
    {
        // BREAD =======================================
        if(prop.HasFlag(PropFlags.Bread))
        {
            IncrementAchievement(ACHIEVEMENT_EAT_5_BREAD);
        }
       
    }

    /// <summary>
    /// Achievements related to little fella
    /// </summary>
    public void ReceivedLittleFella(NewProp prop, int increment)
    {
        if(prop.HasFlag(PropFlags.Bread) || prop.HasFlag(PropFlags.Jam))
        {
            // Feed little fella achievement
            Unlock(ACHIEVEMENT_FEED_LITTLE_FELLA);
        }
    }

    /// <summary>
    /// Achievements relating to toasting
    /// </summary>
    public void ReceivedToasting(NewProp prop, int increment)
    {
        if(prop.HasFlag(PropFlags.Toast))
        {
            Unlock(ACHIEVEMENT_FIRST_TOAST);
        }
    }

    /// <summary>
    /// Achievements related to endings
    /// </summary>
    public void ReceivedEnding()
    {
        Unlock(ACHIEVEMENT_FIRE_ENDING);
    }

    /// <summary>
    /// Achievements relating to Electricity/Explosions
    /// </summary>
    public void ReceivedElectricity(NewProp prop, int increment)
    {
        Unlock(ACHIEVEMENT_ELECTROCUTION);
    }

    public void ReceivedToastNinja(NewProp prop, int increment)
    {
        IncrementToastNinja(ACHIEVEMENT_TOAST_NINJA_SCORE_50, increment);
        IncrementToastNinja(ACHIEVEMENT_TOAST_NINJA_SCORE_100, increment);
    }

    /// <summary>
    /// Achievements relating to Jam
    /// </summary>
    public void ReceivedJam(NewProp prop, int increment)
    {
        if (prop.HasFlag(PropFlags.Jam))
        {
            IncrementAchievement(ACHIEVEMENT_USE_50_JAM);
        }
    }

    /// <summary>
    /// Achievements relating to clock
    /// </summary>
    public void ReceivedTimeChange()
    {
        Unlock(ACHIEVEMENT_USE_CLOCK);
    }

    /// <summary>
    /// Achievements relating to spreads
    /// </summary>
    public void ReceivedSpread(NewProp prop, int increment)
    {
        // Avocado Toast
        if(prop.HasAttribute(StatAttManager.instance.avocadoSpreadAtt))
        {
            Unlock(ACHIEVEMENT_AVOCADO_TOAST);
        }

        // Butter
        if(prop.HasAttribute(StatAttManager.instance.butterSpreadAtt))
        {
            Unlock(ACHIEVEMENT_BUTTER_BREAD);
        }
    }

    void IncrementAchievement(Achievement achievement, int increment = 1)
    {
        // Not tracking achievements, don't do anything
        if(!TrackingAchievements)
        {
            return;
        }

        // Check that achievement has goal and that it is greater than 0
        if(achievement.HasNumericGoal && achievement.AchievementGoal > 0 && !achievement.IsUnlocked)
        {
            // Increase progress
            achievement.AchievementProgress += increment;

            // Update visual progress
            achievement.MenuSquare.ProgressText = $"{achievement.AchievementProgress}/{achievement.AchievementGoal}";

            if(achievement.MenuSquare.IsHiddenAchievement)
            {
                achievement.MenuSquare.NameText = achievement.AchievementName;
                achievement.MenuSquare.DescriptionText = achievement.Description;
            }

            // If goal has been reached, unlock
            if(achievement.AchievementProgress >= achievement.AchievementGoal)
            {
                Unlock(achievement);
            }
        }
    }

    void IncrementToastNinja(Achievement achievement, int score)
    {
        // Check that achievement has goal and that it is greater than 0
        if (achievement.HasNumericGoal && achievement.AchievementGoal > 0 && !achievement.IsUnlocked)
        {
            // Increase progress
            if(achievement.AchievementGoal < score)
            {
                achievement.AchievementProgress = score;
            }

            // Update visual progress
            achievement.MenuSquare.ProgressText = $"{achievement.AchievementProgress}/{achievement.AchievementGoal}";

            if (achievement.MenuSquare.IsHiddenAchievement)
            {
                achievement.MenuSquare.NameText = achievement.AchievementName;
                achievement.MenuSquare.DescriptionText = achievement.Description;
            }

            // If goal has been reached, unlock
            if (achievement.AchievementProgress >= achievement.AchievementGoal)
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

            // Link up references between achievement and its display
            newItem.GetComponent<AchievementDisplay>().associatedAchievement = achievements[i];
            achievements[i].MenuSquare = newItem.GetComponent<AchievementDisplay>();

            newItem.transform.SetParent(menuPanel.transform, false);
        }

        UpdateProgressionToast();
    }

    void PlayNotification(Achievement achievement)
    {
        // Change text
        //bannerText.text = $"\"{achievement.AchievementName}\" Unlocked - Click to view achievements";
        bannerText.text = $"\n\"{achievement.AchievementName}\"";

        // Play a sound here

        //
        // Show banner and start timer
        newAchievementPopup.SetTrigger("animate");
        //bannerAnimator.SetTrigger("TriggerAnimateIn");
        //spinningBreadAnimator.SetTrigger("Start");
    }

    /// <summary>
    /// Sorts achievements by ID into the dictionary, used for loading data
    /// </summary>
    private void SortAchievementsById()
    {
        foreach (Achievement achievement in achievements)
        {
            achievementsById[achievement.ID] = achievement;
        }
    }

    /// <summary>
    /// Formats and returns a save string for all achievements
    /// </summary>
    /// <returns>Save string data</returns>
    public string GetAchievementSaveString()
    {
        string dat = "";

        foreach(Achievement achievement in achievements)
        {
            dat += achievementSeparator;
            dat += achievement.ID;
            dat += spacer;
            dat += (achievement.IsUnlocked == true ? "1" : "0");
            dat += spacer;
            dat += achievement.AchievementProgress;
        }

        dat = dat.Substring(1, dat.Length-1);

        return dat;
    }

    /// <summary>
    /// Loads in a string of data and sets achievement values based on it
    /// </summary>
    /// <param name="data">Achievement save string</param>
    public void LoadAchievementSaveString(string data)
    {
        SortAchievementsById();
        unlockedAchievements = new List<Achievement>();
        string[] allAch = data.Split(achievementSeparator);
        foreach(string a in allAch)
        {
            if(a == "")
            {
                continue;
            }

            string[] aDat = a.Split(spacer);
            int aId = int.Parse(aDat[0]);
            bool aUnlocked = int.Parse(aDat[1]) == 1;
            int aProgress = int.Parse(aDat[2]);

            // Debug.Log($"Loading achievement, id {aId}, complete: {aUnlocked}, current progress: {aProgress}");

            Achievement target = achievementsById[aId];
            target.AchievementProgress = aProgress;
            target.IsUnlocked = aUnlocked;
            if (aUnlocked)
            {
                unlockedAchievements.Add(target);
            }
        }
        UpdateProgressionToast();
    }

    /// <summary>
    /// Sets whether or not achievement tracking should be active
    /// </summary>
    /// <param name="tracking"></param>
    public void SetAchievementTracking(bool tracking)
    {
        TrackingAchievements = tracking;
    }
}
