using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New TN Pattern Wave", menuName = "Minigames/Toast Ninja/Firing Patterns/Wave", order = 55)]
public class TN_Pattern_Wave : TN_FiringPatterns
{
    // ------------------------------- Variables -------------------------------

    [SerializeField, Range(0,10), ValidateInput("ValidateIndex", "index out of range")]
    protected int index = 5;
    [SerializeField]
    protected float timeBetweenShots = .2f;
    [SerializeField]
    protected int step = 1;

    // ------------------------------- Functions -------------------------------

    public override void Launch(ToastNinja toastNinja)
    {
        toastNinja.StartCoroutine(InitialFire(toastNinja, index));
    }

    IEnumerator InitialFire(ToastNinja toastNinja, int index)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        launchers[index].Launch(toastNinja.RandPrefab());

        yield return new WaitForSeconds(timeBetweenShots);
        if (ValidateIndex(index - step))
        {
            toastNinja.StartCoroutine(Fire(toastNinja, index - step, -1));
        }
        if (ValidateIndex(index + step))
        {
            toastNinja.StartCoroutine(Fire(toastNinja, index + step, 1));
        }
    }

    IEnumerator Fire(ToastNinja toastNinja, int index, int polarity)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        launchers[index].Launch(toastNinja.RandPrefab());

        index += polarity * step;

        if (ValidateIndex(index))
        {
            yield return new WaitForSeconds(timeBetweenShots);
            toastNinja.StartCoroutine(Fire(toastNinja, index, polarity * step));
        }

        yield return new WaitForSeconds(0);
    }
}
