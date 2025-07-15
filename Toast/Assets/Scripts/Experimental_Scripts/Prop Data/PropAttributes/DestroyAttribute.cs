using Unity.VisualScripting;
using UnityEngine;
using Steamworks;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;
using System.Runtime.CompilerServices;

[CreateAssetMenu(fileName = "New Destroy Attribute", menuName = "Prop/Attribute/Destroy", order = 53)]
public class DestroyAttribute : PropAttributeSO
{
    [SerializeField]
    private GameObject onEatParticles;

    [SerializeField]
    private Color particleColor;

    public override void OnEquip(NewProp newProp)
    {
        EatWhole(newProp);
    }

    public override void OnRemove(NewProp newProp)
    {

    }

    private void EatWhole(NewProp newProp)
    {
        if (onEatParticles != null)
        {
            GameObject particles = Instantiate(onEatParticles);

            particles.transform.position = newProp.transform.position;

            float toastiness = 0;

            if (newProp.Stats.GetStat(StatAttManager.instance.toastType) != null)
            {
                toastiness = newProp.Stats.GetStat(StatAttManager.instance.toastType).Value;
            }

            toastiness = Mathf.Clamp(toastiness, 0f, 1f);

            Color toastColor = new Color(1 - toastiness, 1 - toastiness, 1 - toastiness) * particleColor;

            var main = particles.GetComponent<ParticleSystem>().main;
            main.startColor = toastColor;
            main.startSizeMultiplier = main.startSizeMultiplier * newProp.transform.localScale.x;

            particles.GetComponent<ParticleCollisionSpawnCrumb>().toastiness = toastiness;
            particles.GetComponent<ParticleCollisionSpawnCrumb>().sizeMult = newProp.transform.localScale.x;
        }

        // Hide use info
        PieManager.instance.StopHover.RaiseEvent(newProp, 1);

        Destroy(newProp.gameObject);
    }
}
