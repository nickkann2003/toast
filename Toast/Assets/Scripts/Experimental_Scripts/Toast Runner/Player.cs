using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ObstacleManager obstacleManagerRef;
    private List<Collider> touchingColliders = new List<Collider>();

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called near the end of the animation to reset the trigger for jumping
    /// </summary>
    public void JumpBuffer()
    {
        playerAnimator.ResetTrigger("jump");
    }

    /// <summary>
    /// Sets the jump trigger for the player
    /// </summary>
    public void Jump()
    {
        playerAnimator.SetTrigger("jump");
    }

    /// <summary>
    /// Checks the players collider for obstacle zones, 
    /// </summary>
    public void CollisionCheck()
    {
        if (touchingColliders.Count > 0)
        {
            Debug.Log("Calculated jump speed");
            // Animation speed, equal to (distX/speedX)/initialTime
            float initialDist = 1f * obstacleManagerRef.ObstacleSpeed;
            float distX = Vector3.Distance(touchingColliders[0].GetComponent<JumpCollider>().jumpTarget.transform.position, transform.position);
            float aSp = 1f / (distX / initialDist);
            playerAnimator.speed = aSp;
        }
        else
        {
            Debug.Log("No colliders found, not calculating");
            playerAnimator.speed = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ToastRunner")
            touchingColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ToastRunner")
            touchingColliders.Remove(other);
    }
}
