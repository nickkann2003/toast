/*
 * Send To Station script - Author: Nick Kannenberg
 * 
 * Small script to enable sending the player to a specific station
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToStation : MonoBehaviour
{
    [SerializeField]
    private Station targetStation;

    public void TriggerSend()
    {
        StationManager.instance.MoveToStation(targetStation);
    }
}