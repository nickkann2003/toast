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

    float timer = 0.0f;

    [SerializeField] private VolumeProfile filterProfile;
    [SerializeField] private VolumeProfile globalProfile;
    [SerializeField] private Volume globalVolume;

    [SerializeField] VoidGameEvent endingEvent;

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
        if(timer > 0)
        {
            timer -= 0.1f * Time.deltaTime;
        }
    }

    void TriggerEnding()
    {
        Debug.Log("Detergent Ending Triggered");
    }

    /// <summary>
    /// After eating bread, the poison level is increased and camera filter is changed
    /// </summary>
    public void IncreasePoison(NewProp prop, int increment)
    {
        if (prop.HasFlag(PropFlags.Bread)) // && prop.HasAttribute(StatAttManager.instance.HasDetergent
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
                break;

            // Ate detergent twice
            case 2:
                break;
                
            // Ate detergent 3 times, time to end the game
            case 3:
                break;
        }
    }
}
