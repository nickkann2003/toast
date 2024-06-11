using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SubsystemsImplementation;
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
    private GameObject notificationBanner;
    [SerializeField]
    TextMeshProUGUI bannerText;

    // ------------------------------- Save Variables -------------------------------
    private string achievementSeparator = "~";
    private string spacer = "_";

    //Temporary before animation, remove later********
    float bannerShowTime;

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
        bannerShowTime += Time.deltaTime;

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
            achievement.MenuSquare.displayImage.sprite = achievement.MenuSquare.unlockedSprite;
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
    public void ReceivedEat(NewProp prop, int increment)
    {
        // BREAD =======================================
        if(prop.HasAttribute(PropFlags.Bread))
        {
            IncrementAchievement(ACHIEVEMENT_EAT_5_BREAD);
        }
       
    }

    void IncrementAchievement(Achievement achievement)
    {
        // Check that achievement has goal and that it is greater than 0
        if(achievement.HasNumericGoal && achievement.AchievementGoal > 0 && !achievement.IsUnlocked)
        {
            // Increase progress
            achievement.AchievementProgress++;

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
    }

    void PlayNotification(Achievement achievement)
    {
        // Change text
        bannerText.text = $"Achievement Unlocked: {achievement.AchievementName} ({achievement.Description}) - Click to view achievements";
        // Play a sound here

        //
        // Show banner and start timer
        notificationBanner.SetActive(true);
        bannerShowTime = 0;
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
        }

        //SortObjectivesById();
        //string[] allObjs = fileDat.Split(objectiveMarker);
        //foreach (string ob in allObjs)
        //{
        //    if (ob.Equals(""))
        //    {
        //        continue;
        //    }
        //
        //    string[] tempObj = ob.Split(requirementStartMarker);
        //    string[] objDatSplit = tempObj[0].Split(spacer);
        //    int tId = int.Parse(objDatSplit[0]);
        //    bool tComplete = objDatSplit[1].Equals("1") ? true : false;
        //    bool tAvailable = objDatSplit[2].Equals("1") ? true : false;
        //
        //    if (tComplete)
        //    {
        //        ObjectivesById[tId].ForceCompleteObjective();
        //        continue;
        //    }
        //
        //    if (tAvailable)
        //    {
        //        string[] reqs = tempObj[1].Split(requirementSpace);
        //        foreach (string req in reqs)
        //        {
        //            string[] reqsSplit = req.Split(spacer);
        //            objectivesById[tId].SetRequirement(int.Parse(reqsSplit[0]), (reqsSplit[1].Equals("1") ? true : false), int.Parse(reqsSplit[2]));
        //        }
        //    }
        //}
        //UpdateText();
    }
}
