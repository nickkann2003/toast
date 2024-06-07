using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rigidbody", menuName = "Prop/Rigidbody", order = 53)] // adds Rigidbody as an asset in the asset menu under the Prop menu
public class PD_Rigidbody : ScriptableObject
{
    [MinValue(.001f)]
    public float mass = 1.0f;
    [MinValue(0)]
    public float drag = .5f;
    [MinValue(0)]
    public float angularDrag = 1.5f;
    public bool useGravity = true;

    [Dropdown("GetCollisionDetectionMode")]
    public CollisionDetectionMode collisionDetection = CollisionDetectionMode.Continuous;

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