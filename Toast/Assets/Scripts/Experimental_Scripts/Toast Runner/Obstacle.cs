using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private BoxCollider jumpOnZone;
    [SerializeField] private BoxCollider jumpOverZone;
    [SerializeField] private BoxCollider collisionZone;
    [SerializeField] private Transform landOnPosition;
    [SerializeField] private Transform landOverPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(landOnPosition.position, 0.03f);
        Gizmos.DrawCube(jumpOnZone.center + jumpOnZone.transform.position, jumpOnZone.size);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(landOverPosition.position, 0.03f);
        Gizmos.DrawCube(jumpOverZone.center + jumpOverZone.transform.position, jumpOverZone.size);

        Gizmos.color = Color.black;
        Gizmos.DrawCube(collisionZone.center + collisionZone.transform.position, collisionZone.size);
    }
}
