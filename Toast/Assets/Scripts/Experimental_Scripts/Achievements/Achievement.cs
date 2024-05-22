using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    private string achievementName; // The name of the achievement
    private string description; // The description of the achievement
    private bool isUnlocked; // Is the achievement unlocked?

    public string AchievementName { get{ return achievementName; } }
    public string Description { get { return description; } }
    public bool IsUnlocked 
    {
        get { return isUnlocked; }
        set { isUnlocked = value; } 
    }

    public Achievement(string name, string description)
    {
        this.achievementName = name;
        this.description = description;
    }
}
