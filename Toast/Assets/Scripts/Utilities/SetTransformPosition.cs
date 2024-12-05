using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransformPosition : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    [Button]
    public void resetPos() { position = transform.position; }
    
    public void SetPos()
    {
        transform.position = position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(position, 0.05f);
    }
}
