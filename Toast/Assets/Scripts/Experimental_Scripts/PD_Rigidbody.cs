using NaughtyAttributes;
using NaughtyAttributes.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rigidbody", menuName = "Prop/Rigidbody", order = 53)] // adds Rigidbody as an asset in the asset menu under the Prop menu
public class PD_Rigidbody : ScriptableObject
{
    [MinValue(.01f)]
    public float mass;
    [MinValue(0)]
    public float drag;
    [MinValue(0)]
    public float angularDrag;
    public bool useGravity;

    [Dropdown("GetCollisionDetectionMode")]
    public CollisionDetectionMode collisionDetection;

    private DropdownList<CollisionDetectionMode> GetCollisionDetectionMode()
    {
        return new DropdownList<CollisionDetectionMode>()
        {
            { "Discrete",   CollisionDetectionMode.Discrete },
            { "Continuous",    CollisionDetectionMode.Continuous },
            { "Continuous Dynamic",    CollisionDetectionMode.ContinuousDynamic },
            { "Continuous Speculative",    CollisionDetectionMode.ContinuousSpeculative }
        };
    }
}