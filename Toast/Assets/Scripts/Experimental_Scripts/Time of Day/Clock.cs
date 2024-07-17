using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public ClockHand hourHand;
    public ClockHand minuteHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceivedHandRotate(GameObject hand)
    {
        if (hand == hourHand.gameObject)
        {
            // Hour hand moved, move minute hand by corresponding amount
        }
        else if (hand == minuteHand.gameObject)
        {
            // Minute hand moved, move hour hand by corresponding amount
        }
        else
        {
            // Somehow something else called this event, return
            return;
        }
    }
}
