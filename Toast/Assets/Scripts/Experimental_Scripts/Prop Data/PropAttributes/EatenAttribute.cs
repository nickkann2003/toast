using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "New Eat Attribute", menuName = "Prop/Attribute/Eat", order = 53)]
public class EatenAttribute : PropAttributeSO
{
    [SerializeField]
    private GameObject onEatParticles;

    [SerializeField]
    private Color particleColor;

    [SerializeField]
    private PropIntGameEvent eatEvent;

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
            //if (newProp.HasAttribute(StatAttManager.instance.inHandAtt))
            //{
            //    particles.transform.position = Camera.main.transform.position;
            //}
            //else
            //{
            //    particles.transform.position = newProp.transform.position;
            //}

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

        if (eatEvent)
        {
            eatEvent.RaiseEvent(newProp, 1);
        }

        // Hide use info
        PieManager.instance.StopHover.RaiseEvent(newProp, 1);

        Destroy(newProp.gameObject);
    }
}
