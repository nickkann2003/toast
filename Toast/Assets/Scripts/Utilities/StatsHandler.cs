using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsHandler
{
    private int breadEaten = 0;
    private int jamUsed = 0;
    private int toastMade = 0;
    private int littleFellaGiven = 0;
    private int toastNinjaHighScore = 0;
    private int objectivesComplete = 0;
    private int achievementsComplete = 0;
    private string parser = ",";

    public int BreadEaten { get => breadEaten; set => breadEaten = value; }
    public int JamUsed { get => jamUsed; set => jamUsed = value; }
    public int ToastMade { get => toastMade; set => toastMade = value; }
    public int LittleFellaGiven { get => littleFellaGiven; set => littleFellaGiven = value; }
    public int ToastNinjaHighScore { get => toastNinjaHighScore; set => toastNinjaHighScore = value; }
    public int ObjectivesComplete { get => objectivesComplete; set => objectivesComplete = value; }
    public int AchievementsComplete { get => achievementsComplete; set => achievementsComplete = value; }

    public void LoadStats(string stats)
    {
        breadEaten = 0;
        jamUsed = 0;
        toastMade = 0;
        littleFellaGiven = 0;
        toastNinjaHighScore = 0;
        objectivesComplete = 0;
        achievementsComplete = 0;

        try
        {
            string[] parsed = stats.Split(parser);

            // Ordered parse, must mach save
            breadEaten = int.Parse(parsed[0]);
            jamUsed = int.Parse(parsed[1]);
            toastMade = int.Parse(parsed[2]);
            littleFellaGiven = int.Parse(parsed[3]);
            toastNinjaHighScore = int.Parse(parsed[4]);
            objectivesComplete = int.Parse(parsed[5]);
            achievementsComplete = int.Parse(parsed[6]);

        }
        catch
        {
            Debug.Log("Stats save parsed incorrectly, setting all values to 0");
        }
    }
    
    public string SaveStats()
    {
        string saveString = "";

        // Ordered save, must match parse
        saveString = addStat(saveString, breadEaten);
        saveString = addStat(saveString, jamUsed);
        saveString = addStat(saveString, toastMade);
        saveString = addStat(saveString, littleFellaGiven);
        saveString = addStat(saveString, toastNinjaHighScore);
        saveString = addStat(saveString, objectivesComplete);
        saveString = addStat(saveString, achievementsComplete);
        
        return saveString;
    }

    private string addStat(string s, int stat)
    {
        if(s.Length > 0)
        {
            s += parser;
        }
        s += stat.ToString();
        return s;
    }
}
