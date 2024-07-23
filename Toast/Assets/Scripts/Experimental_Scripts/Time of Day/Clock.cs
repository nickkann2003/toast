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
    public ClockHand hourHand;
    public ClockHand minuteHand;

    public ClockTimes currentTime;

    public AMPM currentHalf;

    float hourlyRotation;

    Vector2 currentRange;

    [SerializeField]
    Skybox currentSkybox;
     

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0);
        currentTime = ClockTimes.Time0;
        currentRange = new Vector2(0, 30);
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
            if(currentTime == ClockTimes.Time0)
            {
                currentTime = ClockTimes.Time23;
            }
            else
            {
                SwitchHour();
            }

            currentRange.x -= 30;
            currentRange.y -= 30;
        }
        else if(hourHand.transform.localRotation.eulerAngles.y >= currentRange.y)
        {
            if(currentTime == ClockTimes.Time23)
            {
                currentTime = ClockTimes.Time0;
            }
            else
            {
                SwitchHour();
            }

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
            minuteHand.transform.RotateAround(transform.position, transform.up, ((moveAmount - 360) * 60));

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
        switch(hourHand.transform.localEulerAngles.y)
        {
            // 12
            case float i when i >= 0 && i < 30:
                if(currentTime == ClockTimes.Time11)
                {
                    currentHalf = AMPM.PM;
                }
                
                if(currentTime == ClockTimes.Time23)
                {
                    currentHalf = AMPM.AM;
                }


                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time0;
                }
                else
                {
                    currentTime = ClockTimes.Time12;
                }
                break;
            // 1
            case float i when i >= 30 && i < 60:
                if (currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time1;
                }
                else
                {
                    currentTime = ClockTimes.Time13;
                }
                break;
            // 2
            case float i when i >= 60 && i < 90:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time2;
                }
                else
                {
                    currentTime = ClockTimes.Time14;
                }
                break;
            // 3
            case float i when i >= 90 && i < 120:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time3;
                }
                else
                {
                    currentTime = ClockTimes.Time15;
                }
                break;
            // 4
            case float i when i >= 120 && i < 150:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time4;
                }
                else
                {
                    currentTime = ClockTimes.Time16;
                }
                break;
            // 5
            case float i when i >= 150 && i < 180:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time5;
                }
                else
                {
                    currentTime = ClockTimes.Time17;
                }
                break;
            // 6
            case float i when i >= 180 && i < 210:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time6;
                }
                else
                {
                    currentTime = ClockTimes.Time18;
                }
                break;
            // 7
            case float i when i >= 210 && i < 240:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time7;
                }
                else
                {
                    currentTime = ClockTimes.Time19;
                }
                break;
            // 8
            case float i when i >= 240 && i < 270:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time8;
                }
                else
                {
                    currentTime = ClockTimes.Time20;
                }
                break;
            // 9 
            case float i when i >= 270 && i < 300:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time9;
                }
                else
                {
                    currentTime = ClockTimes.Time21;
                }
                break;
            // 10
            case float i when i >= 300 && i < 330:
                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time10;
                }
                else
                {
                    currentTime = ClockTimes.Time22;
                }
                break;
            // 11
            case float i when i >= 330 && i < 360:

                if (currentTime == ClockTimes.Time0)
                {
                    currentHalf = AMPM.PM;
                }

                if(currentTime == ClockTimes.Time12)
                {
                    currentHalf = AMPM.AM;
                }

                if(currentHalf == AMPM.AM)
                {
                    currentTime = ClockTimes.Time11;
                }
                else
                {
                    currentTime = ClockTimes.Time23;
                }
                break;
            
        }
    }
    
}
