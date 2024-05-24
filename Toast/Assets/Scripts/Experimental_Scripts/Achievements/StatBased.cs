using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBased : Achievement
{
    private int achievementGoal;
    private int currentCount;

    public int AchievementProgress
    {
        get { return currentCount; }
        set { currentCount = value; }
    }

    public int AchievementGoal { get { return achievementGoal; } }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public StatBased(string name, string description, int goal)
        : base(name, description)
    {
        this.achievementGoal = goal;
        currentCount = 0;
    }
}
