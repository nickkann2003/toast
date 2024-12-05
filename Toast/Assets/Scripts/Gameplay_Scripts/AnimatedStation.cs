using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedStation : Station
{
    /// <summary>
    /// Animator for this station
    /// Must have the trigger 'start'
    /// Must trigger this script's 'AnimationOver' function when over
    /// </summary>
    [Header("ANIMATION STATION VARIABLES")]
    public Animator stationAnimator;

    public Station nextStation;

    /// <summary>
    /// Overrides OnArrive, triggers animation, locking station movement while animating
    /// </summary>
    /// <param name="forward"></param>
    public override void OnArrive(bool forward)
    {
        base.OnArrive(forward);
        StationManager.instance.LockStationMovement();
        stationAnimator.SetTrigger("start");
    }

    public void AnimationOver()
    {
        StationManager.instance.UnlockStationMovement();
        StationManager.instance.MoveToStation(nextStation);
    }

    public void StartAnimation()
    {
        StationManager.instance.MoveToStation(this);
    }
}
