using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spread Effect", menuName = "Prop/Use Effect/Spread", order = 53)]
public class USE_Spread : UseEffectSO
{
    // events - borrowed from USE_Jam
    [Header("Event References")]
    [SerializeField]
    private bool invokeEvents;
    [SerializeField, EnableIf("invokeEvents")]
    private PropIntGameEvent useEvent;


    public override bool TryUse(NewProp newProp)
    {
        // Make sure object is a spread
        Spread spread = newProp.GetComponent<Spread>();
        if(spread == null)
        {
            return false;
        }

        // Raycast hit
        RaycastHit hit = Raycast.Instance.RaycastHelper(~(1 << 10) & ~(1 << 3));

        // Check for collider
        if (hit.collider.gameObject != null)
        {
            // Check for prop
            NewProp prop = hit.collider.gameObject.GetComponent<NewProp>();
            if (prop != null)
            {
                // Check for bread
                if (prop.HasFlag(PropFlags.Bread))
                {
                    // Spread application handled in spread class
                    spread.ApplySpread(prop);
                    return true;
                }
            }
        }
        
        return false;
    }
}
