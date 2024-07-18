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
    PM,
}

public class Clock : MonoBehaviour
{
    public ClockHand hourHand;
    public ClockHand minuteHand;

    public ClockTimes currentTime;

    float hourlyRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = ClockTimes.Time0;
        hourlyRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Next time reached
        if (hourlyRotation >= 30)
        {
            currentTime++;
            hourlyRotation = 0;
        }
        // Rewind, previous time reached
        else if (hourlyRotation <= -30)
        {
            currentTime--;
            hourlyRotation = 0;
        }
        // Special case at midnight
        else if(hourlyRotation < 0 && currentTime == ClockTimes.Time0)
        {
            currentTime = ClockTimes.Time23;
        }

        Debug.Log("The current time is " + currentTime.ToString());
    }

    public void ReceivedHandRotate(GameObject hand, float moveAmount)
    {
        if (hand == hourHand.gameObject)
        {
            // Hour hand moved, move minute hand by corresponding amount
            minuteHand.transform.RotateAround(transform.position, transform.up, moveAmount * 60);

            hourlyRotation += moveAmount;
        }
        else if (hand == minuteHand.gameObject)
        {
            // Bootleg fix, Unity sometimes calculates change as negative 360 instead of starting at 0
            if (Mathf.Abs(moveAmount) < 300)
            {
                // Minute hand moved, move hour hand by corresponding amount
                hourHand.transform.RotateAround(transform.position, transform.up, moveAmount / 60);

                hourlyRotation += (moveAmount / 60);
            }
        }
        else
        {
            // Somehow something else called this event, return
            return;
        }

        
    }
}
