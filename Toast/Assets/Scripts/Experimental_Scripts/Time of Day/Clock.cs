using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ClockTimes
{
    Time0,
    Time1,
    Time2,
    Time3,
    Time4,
    Time5,
    Time6,
    Time7,
    Time8,
    Time9,
    Time10,
    Time11,
    Time12,
    Time13,
    Time14,
    Time15,
    Time16,
    Time17,
    Time18,
    Time19,
    Time20,
    Time21,
    Time22,
    Time23
}

public enum AMPM
{
    AM,
    PM
}

public class Clock : MonoBehaviour
{
    [Header("Clock Hands")]
    // The hands of the clock
    public ClockHand hourHand;
    public ClockHand minuteHand;

    // Current hour
    public ClockTimes currentTime;

    // Used to track times when hour hand has made one revolution from 12-12
    public AMPM currentHalf;

    float hourlyRotation;

    // The current range of angles
    Vector2 currentRange;

    [SerializeField]
    Skybox currentSkybox;

    [Header("Skybox Materials")]
    [SerializeField]
    Material daySkybox, nightSkybox, betweenSkybox;

    [Header("Events")]
    public VoidGameEvent timeChangeEvent;    

    /*
    [Header("Directional Lighting")]
    [SerializeField]
    UnityEngine.Light directionalLight;
    */

    [Header("Lighting Colors")]
    [SerializeField]
    Color dayColor, nightColor, sunsetColor, sunriseColor;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
        currentTime = ClockTimes.Time0;
        currentRange = new Vector2(270, 300);
        currentHalf = AMPM.AM;
        hourlyRotation = 0;
    }

    // Update is called once per frame
    void Update()
    { 
        if(currentRange.y > 360)
        {
            // Reset range
            currentRange.x = 0;
            currentRange.y = 30;
        }

        if(currentRange.x < 0)
        {
            currentRange.x = 330;
            currentRange.y = 360;
        }

        if(hourHand.transform.localRotation.eulerAngles.y < currentRange.x)
        {
            SwitchHour();
            
            currentRange.x -= 30;
            currentRange.y -= 30;
        }
        else if(hourHand.transform.localRotation.eulerAngles.y >= currentRange.y)
        {
            SwitchHour();

            currentRange.x += 30;
            currentRange.y += 30;
        }

        //Debug.Log("The current time is " + currentTime.ToString() + " With a rotation of: " + hourlyRotation);
    }

    public void ReceivedHandRotate(GameObject hand, float moveAmount)
    {
        if (hand == hourHand.gameObject)
        {
            // Hour hand moved, move minute hand by corresponding amount
            minuteHand.transform.RotateAround(transform.position, transform.up, (moveAmount * 60));

            hourlyRotation += moveAmount;
        }
        else if (hand == minuteHand.gameObject)
        {
            // Bootleg fix, Unity sometimes calculates change as negative 360 instead of starting at 0
            if(moveAmount < 0)
            {
                moveAmount = 360 + moveAmount;
            }
            else if (moveAmount > 0)
            {
                moveAmount = moveAmount - 360;
            }
          
            // Minute hand moved, move hour hand by corresponding amount
            hourHand.transform.RotateAround(transform.position, transform.up, -moveAmount / 1800);

            hourlyRotation += (-moveAmount / 1800);            
        }
        else
        {
            // Somehow something else called this event, return
            return;
        }

        RenderSettings.skybox.SetFloat("_Rotation", hourHand.gameObject.transform.localRotation.eulerAngles.y);
    }

    void SwitchHour()
    {
        timeChangeEvent.RaiseEvent();

        switch(hourHand.transform.localEulerAngles.y)
        {
            // 12
            case float i when i >= 0 && i < 30:

                // Changing AM/PM
                if(currentTime == ClockTimes.Time11)
                {
                    currentHalf = AMPM.PM;
                }
                if(currentTime == ClockTimes.Time23)
                {
                    currentHalf = AMPM.AM;
                }

                // 12 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time0;
                    RenderSettings.skybox = nightSkybox;
                }
                // 12 PM appearance
                else
                {
                    currentTime = ClockTimes.Time12;
                    RenderSettings.skybox = daySkybox;
                }
                break;
            // 1
            case float i when i >= 30 && i < 60:
                // 1 AM Appearance
                if (currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time1;
                    RenderSettings.skybox = nightSkybox;
                }
                // 1 PM appearance
                else
                {
                    currentTime = ClockTimes.Time13;
                    RenderSettings.skybox = daySkybox;
                }
                break;
            // 2
            case float i when i >= 60 && i < 90:
                // 2 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time2;
                    RenderSettings.skybox = nightSkybox;
                }
                // 2 PM appearance
                else
                {
                    currentTime = ClockTimes.Time14;
                    RenderSettings.skybox = daySkybox;
                }
                break;
            // 3
            case float i when i >= 90 && i < 120:
                // 3 AM Appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time3;
                    RenderSettings.skybox.Lerp(nightSkybox, betweenSkybox, 0.3f);
                    RenderSettings.ambientLight = Color.Lerp(nightColor, sunriseColor, 0.3f);
                }
                // 3 PM appearance
                else
                {
                    currentTime = ClockTimes.Time15;
                    RenderSettings.skybox.Lerp(daySkybox, betweenSkybox, 0.1f);
                    RenderSettings.ambientLight = Color.Lerp(dayColor, sunsetColor, 0.1f);
                }
                break;
            // 4
            case float i when i >= 120 && i < 150:
                // 4 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time4;
                    RenderSettings.skybox.Lerp(nightSkybox, betweenSkybox, 0.5f);
                    RenderSettings.ambientLight = Color.Lerp(nightColor, sunriseColor, 0.5f);
                }
                // 4 PM appearance
                else
                {
                    currentTime = ClockTimes.Time16;
                    RenderSettings.skybox.Lerp(daySkybox, betweenSkybox, 0.3f);
                    RenderSettings.ambientLight = Color.Lerp(dayColor, sunsetColor, 0.3f);
                }
                break;
            // 5
            case float i when i >= 150 && i < 180:
                // 5 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time5;
                    RenderSettings.skybox = betweenSkybox;
                    RenderSettings.ambientLight = sunriseColor;
                }
                // 5 PM appearance
                else
                {
                    currentTime = ClockTimes.Time17;
                    RenderSettings.skybox.Lerp(daySkybox, betweenSkybox, 0.5f);
                    RenderSettings.ambientLight = Color.Lerp(dayColor, sunsetColor, 0.5f);
                }
                break;
            // 6
            case float i when i >= 180 && i < 210:
                // 6 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time6;
                    RenderSettings.skybox.Lerp(betweenSkybox, daySkybox, 0.5f);
                    RenderSettings.ambientLight = Color.Lerp(sunriseColor, dayColor, 0.5f);

                }
                // 6 PM appearance
                else
                {
                    currentTime = ClockTimes.Time18;
                    RenderSettings.skybox.Lerp(daySkybox, betweenSkybox, 0.75f);
                    RenderSettings.ambientLight = Color.Lerp(dayColor, sunsetColor, 0.75f);
                }
                break;
            // 7
            case float i when i >= 210 && i < 240:
                // 7 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time7;
                    RenderSettings.skybox.Lerp(betweenSkybox, daySkybox, 0.8f);
                }
                // 7 PM appearance
                else
                {
                    currentTime = ClockTimes.Time19;
                    RenderSettings.skybox = betweenSkybox;
                    RenderSettings.ambientLight = sunsetColor;
                }
                break;
            // 8
            case float i when i >= 240 && i < 270:
                // 8 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time8;
                    RenderSettings.skybox = daySkybox;
                    RenderSettings.ambientLight = dayColor;
                }
                // 8 PM appearance
                else
                {
                    currentTime = ClockTimes.Time20;
                    RenderSettings.skybox.Lerp(betweenSkybox, nightSkybox, 0.5f);
                    RenderSettings.ambientLight = Color.Lerp(sunsetColor, nightColor, 0.5f);
                }
                break;
            // 9 
            case float i when i >= 270 && i < 300:
                // 9 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time9;
                    RenderSettings.skybox = daySkybox;
                }
                // 9 PM appearance
                else
                {
                    currentTime = ClockTimes.Time21;
                    RenderSettings.skybox = nightSkybox;
                    RenderSettings.ambientLight = nightColor;
                }
                break;
            // 10
            case float i when i >= 300 && i < 330:
                // 10 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time10;
                    RenderSettings.skybox = daySkybox;
                }
                // 10 PM appearance
                else
                {
                    currentTime = ClockTimes.Time22;
                    RenderSettings.skybox = nightSkybox;
                }
                break;
            // 11
            case float i when i >= 330 && i < 360:

                // Changing AM/PM
                if (currentTime == ClockTimes.Time0)
                {
                    currentHalf = AMPM.PM;
                }
                if(currentTime == ClockTimes.Time12)
                {
                    currentHalf = AMPM.AM;
                }

                // 11 AM appearance
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time11;
                    RenderSettings.skybox = daySkybox;
                }
                // 11 PM appearance
                else
                {
                    currentTime = ClockTimes.Time23;
                    RenderSettings.skybox = nightSkybox;
                }
                break;
            
        }
    }

    public void EnvironmentalReset()
    {
        // Reset skybox
        RenderSettings.skybox.SetFloat("_Rotation", 0);

        // Reset clock hands
        hourHand.transform.RotateAround(transform.position, transform.up, 270 - hourHand.transform.localEulerAngles.y);
        minuteHand.transform.RotateAround(transform.position, transform.up, -minuteHand.transform.localEulerAngles.y);

        // Set to AM
        currentHalf = AMPM.AM;

        SwitchHour();

        // Reset light color
        //directionalLight.color = new Color(255,255,255,255);
        //directionalLight.intensity = 0.32f;

        RenderSettings.ambientLight = dayColor;
    }
    
}
