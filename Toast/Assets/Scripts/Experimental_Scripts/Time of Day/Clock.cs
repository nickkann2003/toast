using System.Collections;
using System.Collections.Generic;
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

public class Clock : MonoBehaviour
{
    public ClockHand hourHand;
    public ClockHand minuteHand;

    public ClockTimes currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = ClockTimes.Time0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedHandRotate(GameObject hand, float moveAmount)
    {
        if (hand == hourHand.gameObject)
        {
            // Hour hand moved, move minute hand by corresponding amount
            minuteHand.transform.RotateAround(transform.position, transform.up, moveAmount * 60);

            Debug.Log("Moving Hour Hand");
        }
        else if (hand == minuteHand.gameObject)
        {
            // Bootleg fix, Unity sometimes calculates change as negative 360 instead of starting at 0
            if (Mathf.Abs(moveAmount) < 300)
            {
                // Minute hand moved, move hour hand by corresponding amount
                hourHand.transform.RotateAround(transform.position, transform.up, moveAmount / 12);
            }
        }
        else
        {
            // Somehow something else called this event, return
            return;
        }
    }
}
