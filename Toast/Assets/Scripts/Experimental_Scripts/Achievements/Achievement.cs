using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Diagnostics;

[CreateAssetMenu(fileName ="New Achievement", menuName ="Achievement", order = 51)]
public class Achievement : ScriptableObject
{
    [Header("Achievement Text Information")]
    [SerializeField]
    private string achievementName; // The name of the achievement
    [SerializeField]
    private string description; // The description of the achievement

    [Header("Unlocked Status")]
    [SerializeField]
    private bool isUnlocked; // Is the achievement unlocked?

    [Header("Does the achievement have a target number?")]
    [SerializeField]
    private bool hasNumericGoal; // Does the achievement have a target number of actions that must be reached?

    [Header("Goal amount")]
    [SerializeField,ShowIf("hasNumericGoal")]
    private int achievementGoal; // The target number for the achievement

    [Header("Progress Towards Goal")]
    [SerializeField,ReadOnly,ShowIf("hasNumericGoal")]
    private int currentCount; // The current progress towards the target

    [Header("Achievement ID Number")]
    [SerializeField,ReadOnly]
    private int id;

    public int AchievementProgress
    {
        get { return currentCount; }
        set { currentCount = value; }
    }

    public int AchievementGoal { get { return achievementGoal; } }

    public string AchievementName { get{ return achievementName; } }
    public string Description { get { return description; } }
    public bool IsUnlocked 
    {
        get { return isUnlocked; }
        set { isUnlocked = value; } 
    }

    public bool HasNumericGoal
    {
        get { return hasNumericGoal; }
        set { hasNumericGoal = value; }
    }

    public int ID { get { return id; } }

    /*
    /// <summary>
    /// Non-stat-based achievements with only a name and descriptiom
    /// </summary>
    /// <param name="name">The name of the achievement</param>
    /// <param name="description">The description of the actions needed to unlock the achievement Ex: "Clear the tutorial"</param>
    public Achievement(string name, string description)
    {
        this.achievementName = name;
        this.description = description;
        hasNumericGoal = false;
        achievementGoal = 0;
        currentCount = 0;
    }

    /// <summary>
    /// Achievments with numeric goals
    /// </summary>
    /// <param name="name">The name of the achievement</param>
    /// <param name="description">The description of the actions needed to unlock the achievement Ex: "Eat 5 Bread"</param>
    /// <param name="achievementGoal">The number of actions that need to be reached to unlock the achievement</param>
    public Achievement(string name, string description, int achievementGoal)
    {
        this.achievementName = name;
        this.description = description;
        this.achievementGoal = achievementGoal;
        hasNumericGoal = true;
        currentCount = 0;
    }
    */
}
