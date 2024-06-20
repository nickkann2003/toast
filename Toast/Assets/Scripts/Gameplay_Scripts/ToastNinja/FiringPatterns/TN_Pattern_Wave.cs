using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New TN Pattern Wave", menuName = "Minigames/Toast Ninja/Firing Patterns/Wave", order = 55)]
public class TN_Pattern_Wave : TN_FiringPatterns
{
    // ------------------------------- Variables -------------------------------

    [SerializeField, Range(0,10)]
    protected int centralIndex = 5;
    [SerializeField]
    protected float timeBetweenShots = .2f;
    [SerializeField, MinValue(1)]
    protected int step = 1;
    [SerializeField]
    protected int startDistance = 0;

    // ------------------------------- Functions -------------------------------

    public override void Launch(ToastNinja toastNinja)
    {
        toastNinja.StartCoroutine(InitialFire(toastNinja, startDistance));
    }

    IEnumerator InitialFire(ToastNinja toastNinja, int distanceFromCenter)
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
        }
        else
        {
            launchers[centralIndex].Launch(toastNinja.RandPrefab());
        }

        distanceFromCenter += step;

        if (ValidateIndex(centralIndex + distanceFromCenter) || ValidateIndex(centralIndex - distanceFromCenter))
        {
            yield return new WaitForSeconds(timeBetweenShots);
            toastNinja.StartCoroutine(InitialFire(toastNinja, distanceFromCenter));
        }

        yield return null;
    }

    IEnumerator Fire(ToastNinja toastNinja, int index, int polarity)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        launchers[index].LaunchSO(RandomPrefab());

        index += polarity * step;

        if (ValidateIndex(index))
        {
            yield return new WaitForSeconds(timeBetweenShots);
            toastNinja.StartCoroutine(Fire(toastNinja, index, polarity * step));
        }

        yield return null;
    }
}
