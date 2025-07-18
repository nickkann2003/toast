using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DetergentManager : MonoBehaviour
{
    public static DetergentManager instance;

    // Requires 3 levels to reach ending
    int poisonLevel = 0;

    [SerializeField]
    float poisonCooldownCount;

    [SerializeField]
    float timer = 0.0f;

    [SerializeField] private VolumeProfile filterProfileLvl1, filterProfileLvl2, filterProfileLvl3;
    [SerializeField] private VolumeProfile globalProfile;
    [SerializeField] private Volume globalVolume;

    [SerializeField] VoidGameEvent endingEvent;

    [SerializeField] Station endingStation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        poisonLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Cooldown has been reached, reset poison
        if (timer <= 0)
        {
            poisonLevel = 0;
            ApplyFilters();
        }

        // Countdown if able
        if(timer > 0 && poisonLevel < 3)
        {
            timer -= 1.0f * Time.deltaTime;
        }
    }

    void TriggerEnding()
    {
        // Play ending "animation"
        Time.timeScale = 0.2f;

        // Trigger event
        endingEvent.RaiseEvent();
        
        StationManager.instance.playerPath.Clear();
        StationManager.instance.MoveToStation(endingStation);

        StartCoroutine(GiveEnding());
    }

    /// <summary>
    /// After eating bread, the poison level is increased and camera filter is changed
    /// </summary>
    public void IncreasePoison(NewProp prop, int increment)
    {
        if (prop.HasFlag(PropFlags.Bread) && prop.HasAttribute(StatAttManager.instance.hasDetergentAtt))
        {
            poisonLevel++;
            ApplyFilters();
            // If condition is met, trigger ending
            if (poisonLevel >= 3)
            {
                TriggerEnding();
            }
            timer = poisonCooldownCount;
        }
        
    }

    /// <summary>
    /// Determines how to change the camera display based on how much detergent has been eaten
    /// </summary>
    void ApplyFilters()
    {
        switch(poisonLevel)
        {
            // Not poisoned
            case 0:
                globalVolume.profile = globalProfile;
                break;

            // Ate detergent once
            case 1:
                globalVolume.profile = filterProfileLvl1;
                break;

            // Ate detergent twice
            case 2:
                globalVolume.profile = filterProfileLvl2;
                break;
                
            // Ate detergent 3 times, time to end the game
            case 3:
                globalVolume.profile = filterProfileLvl3;
                break;
        }
    }

    private IEnumerator GiveEnding()
    {
        yield return new WaitForSecondsRealtime(3);

        GameManager.Instance.LoadGame(0);
    }
}
