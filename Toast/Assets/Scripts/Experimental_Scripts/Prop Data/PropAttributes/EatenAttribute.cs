using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "New Eat Attribute", menuName = "Prop/Attribute/Eat", order = 53)]
public class EatenAttribute : PropAttributeSO
{
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
        if (eatEvent)
        {
            eatEvent.RaiseEvent(newProp, 1);
        }

        Destroy(newProp.gameObject);
    }
}
