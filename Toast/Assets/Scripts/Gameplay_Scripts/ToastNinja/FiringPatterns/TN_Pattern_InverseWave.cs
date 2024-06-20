using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Pattern Inverse Wave", menuName = "Minigames/Toast Ninja/Firing Patterns/Inverse Wave", order = 55)]
public class TN_Pattern_InverseWave : TN_FiringPatterns
{
    [SerializeField, Range(0, 10)]
    protected int centralIndex = 5;
    [SerializeField]
    protected float timeBetweenShots = .2f;
    [SerializeField, MinValue(1)]
    protected int step = 1;
    [SerializeField]
    protected int maxIterations = 5;
    [SerializeField]
    protected int startDistance = 2;

    public override void Launch(ToastNinja toastNinja)
    {
        toastNinja.StartCoroutine(Fire(toastNinja, startDistance, maxIterations));
    }

    IEnumerator Fire(ToastNinja toastNinja, int distanceFromCenter, int iteration)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        //AudioManager.instance.PlayOneShotSound(AudioManager.instance.launch);

        if (distanceFromCenter > 0)
        {
            int leftIndex = centralIndex - distanceFromCenter;
            int rightIndex = centralIndex + distanceFromCenter;

            if (ValidateIndex(leftIndex))
            {
                launchers[leftIndex].LaunchSO(RandomPrefab());
            }
            if (ValidateIndex(rightIndex))
            {
                launchers[rightIndex].LaunchSO(RandomPrefab());
            }

            distanceFromCenter -= step;
            iteration--;

            if (distanceFromCenter < 0)
            {
                distanceFromCenter = 0;
            }

            if (iteration > 0)
            {
                yield return new WaitForSeconds(timeBetweenShots);
                toastNinja.StartCoroutine(Fire(toastNinja, distanceFromCenter, iteration));
            }
            else
            {
                yield return null;
            }
        }
        else
        {
            yield return null;
            launchers[centralIndex].Launch(toastNinja.RandPrefab());
        }
    }
}
