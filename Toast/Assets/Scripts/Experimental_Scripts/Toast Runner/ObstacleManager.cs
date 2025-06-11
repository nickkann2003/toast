/*
 * Author: Nick Kannenberg
 * 
 * ObstacleManager.cs
 * 
 * Obstacle manager class for all obstacles within the toast runner minigame.
 * Handles creating, updating, and destroying obstacles within the minigame.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Is this manager active?
    public bool active = true;

    /// <summary>
    /// List of all obstacle prefabs that can be created as part of the game
    /// </summary>
    [SerializeField] private List<GameObject> obstaclePrefabs = new List<GameObject>();

    /// <summary>
    /// List of all currently existing obstacles within the minigame
    /// </summary>
    [SerializeField] private List<GameObject> obstacles = new List<GameObject>();

    /// <summary>
    /// Speed of the obstacles in the minigame, in world units per second
    /// </summary>
    [SerializeField] private float obstacleSpeed = 0.1f;
    public float ObstacleSpeed { get => obstacleSpeed; }

    /// <summary>
    /// Acceleration of game speed, in world units per second per second
    /// </summary>
    [SerializeField] private float acceleration = 0.0005f;

    /// <summary>
    /// Total distance traveled, in world units
    /// </summary>
    [SerializeField] private float distance = 0f;

    /// <summary>
    /// Distance interval for spawning objects, in world units
    /// </summary>
    [SerializeField] private float obstacleInterval = 2.5f;
    private float intDistRemaining = 0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            // Get delta time in seconds
            float dTime = Time.deltaTime;

            // Increase obstacle speed by acceleration
            obstacleSpeed += acceleration * dTime;

            // Increase interval distance and distance
            distance += obstacleSpeed * dTime;
            intDistRemaining -= obstacleSpeed * dTime;

            // Get diff vector
            Vector3 diff = new Vector3(-obstacleSpeed * dTime, 0, 0);

            // Move obstacles
            foreach(GameObject o in obstacles)
            {
                o.transform.position += diff;
            }

            // Check distance spawn interval
            if(intDistRemaining <= 0)
            {
                intDistRemaining = obstacleInterval;
                SpawnObstacle(0);
            }
        }
    }

    /// <summary>
    /// Spawns an obstacle and adds it to the list of obstacles
    /// </summary>
    /// <param name="index">Index in the obstacle prefab list to spawn</param>
    private void SpawnObstacle(int index)
    {
        GameObject obj = Instantiate(obstaclePrefabs[index], transform);
        obj.transform.localPosition = Vector3.zero;
        obstacles.Add(obj);
    }
}
