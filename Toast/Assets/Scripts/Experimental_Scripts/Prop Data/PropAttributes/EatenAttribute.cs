using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "New Eat Attribute", menuName = "Prop/Attribute/Eat", order = 53)]
public class EatenAttribute : PropAttributeSO
{
    [SerializeField]
    private GameObject onEatParticles;

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

            float size = Mathf.Clamp(newProp.transform.localScale.x, .5f, 3.0f);
            particles.transform.localScale = Vector3.one * size;
            var main = particles.GetComponent<ParticleSystem>().main;

            float toastiness = newProp.Stats.GetStat(StatAttManager.instance.toastType).Value;
        }

        if (eatEvent)
        {
            eatEvent.RaiseEvent(newProp, 1);
        }

        Destroy(newProp.gameObject);
    }
}
