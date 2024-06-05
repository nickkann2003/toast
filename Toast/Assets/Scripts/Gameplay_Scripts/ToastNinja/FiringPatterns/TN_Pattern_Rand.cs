using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Pattern Rand", menuName = "Minigames/Toast Ninja/Firing Patterns/Random", order = 55)]
public class TN_Pattern_Rand : TN_FiringPatterns
{
    [SerializeField]
    private int num = 1;
    [SerializeField]
    private float timeBetweenShots = .3f;

    public override void Launch(ToastNinja toastNinja)
    {
        toastNinja.StartCoroutine(Fire(toastNinja, num));
        //Fire(toastNinja, num);
    }

    IEnumerator Fire(ToastNinja toastNinja, int amount)
    {
        LaunchObject[] launchers = toastNinja.LaunchObjects;

        int launcher = Random.Range(0, launchers.Length);

        launchers[launcher].Launch(toastNinja.RandPrefab());

        amount--;

        if (amount > 0)
        {
            yield return new WaitForSeconds(timeBetweenShots);
            toastNinja.StartCoroutine(Fire(toastNinja, amount));
        }

        yield return new WaitForSeconds(0);
    }
}
