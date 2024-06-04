using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Pattern Wave", menuName = "Minigames/Toast Ninja/Firing Patterns/Wave", order = 55)]
public class TN_Pattern_Wave : TN_FiringPatterns
{
    [SerializeField]
    private int index = 5;
    [SerializeField]
    private float timeBetweenShots = .3f;

    public override void Launch(ToastNinja toastNinja)
    {
        toastNinja.StartCoroutine(InitialFire(toastNinja, index));
    }

    IEnumerator InitialFire(ToastNinja toastNinja, int index)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        launchers[index].Launch(toastNinja.RandPrefab());

        yield return new WaitForSeconds(timeBetweenShots);
        if (index -1 >= 0)
        {
            toastNinja.StartCoroutine(Fire(toastNinja, index - 1, -1));
        }
        if (index + 1 < launchers.Length)
        {
            toastNinja.StartCoroutine(Fire(toastNinja, index + 1, 1));
        }
    }

    IEnumerator Fire(ToastNinja toastNinja, int index, int add)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        launchers[index].Launch(toastNinja.RandPrefab());

        index += add;

        if (index >= 0 && index < launchers.Length)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            toastNinja.StartCoroutine(Fire(toastNinja, index, add));
        }

        yield return new WaitForSeconds(0);
    }
}
