using UnityEngine;

[CreateAssetMenu(fileName = "New Metal Attribute", menuName = "Prop/Attribute/Metal", order = 53)]
public class MetalAttribute : PropAttributeSO
{
    public override void OnEquip(NewProp newProp)
    {
        newProp.AddFlag(PropFlags.Metal);
        // ADD METALLIC TEXTURE

        // ADD EXPLOSIVE PROPERTIES
    }

    public override void OnRemove(NewProp newProp)
    {
        // REMOVE METALLIC TEXTURE

        // REMOVE EXPLOSIVE PROPERTIES
    }
}
