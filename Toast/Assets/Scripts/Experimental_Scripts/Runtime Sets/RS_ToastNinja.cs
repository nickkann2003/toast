using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN RuntimeSet", menuName = "RuntimeSet/ToastNinja", order = 53)]
public class RS_ToastNinja : RuntimeSet<TN_Object>
{
    public void DestroyAll()
    {
        if (items.Count <= 0) return;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] != null)
            {
                Destroy(items[i].gameObject);
            }
        }
    }
}
