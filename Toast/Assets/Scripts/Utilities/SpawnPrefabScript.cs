using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnPrefabScript : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------------ Item to Spawn --------------")]
    public GameObject prefab;
    
    [Header("----------------- Spawn Variables -------------")]
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    private Vector3 actualSpawnPos;

    public Vector3 spawnRandomness;
    public Quaternion spawnRotationRandomness;

    [SerializeField]
    private bool automaticSpawning;
    [Header("----------------- Automatic Spawning -----------------"), ShowIf("automaticSpawning")]
    [SerializeField]
    private float spawnRatePerSecond;
    private float spawnDelay;
    private float cd;

    // ------------------------------- Functions -------------------------------
    public void Start()
    {
        actualSpawnPos = spawnPosition + transform.position;
        spawnDelay = 1f / spawnRatePerSecond;
    }

    private void Update()
    {
        if (automaticSpawning && gameObject.activeSelf)
        {
            cd -= Time.deltaTime;
            if (cd < 0f)
            {
                TriggerSpawn();
                cd = spawnDelay;
            }
        }
    }

    /// <summary>
    /// Triggers the spawn of this prefab
    /// </summary>
    public void TriggerSpawn()
    {
        Vector3 randomness = new Vector3(Random.value*spawnRandomness.x, Random.value*spawnRandomness.y, Random.value*spawnRandomness.z);
        Quaternion rotationRandomness = new Quaternion(Random.value*spawnRotationRandomness.x + spawnRotation.x, Random.value*spawnRotationRandomness.y + spawnRotation.y, Random.value*spawnRotationRandomness.z + spawnRotation.z, spawnRotation.w);
        GameObject spawnedObject = Instantiate(prefab);
        
        spawnedObject.transform.position = actualSpawnPos + randomness;
        spawnedObject.transform.rotation = rotationRandomness;
    }

    /// <summary>
    /// Sets the automatic spawn rate of this spawner
    /// </summary>
    /// <param name="rate">New rate, in spawns per second</param>
    public void SetAutomaticSpawnRate(float rate)
    {
        spawnRatePerSecond = rate;
        spawnDelay = 1f / spawnRatePerSecond;
    }

    /// <summary>
    /// Sets this spawners prefab to a given prefab
    /// </summary>
    /// <param name="prefab">New prefab</param>
    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }

    /// <summary>
    /// Gizmos, shows red square for position with randomness
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnPosition + transform.position, spawnRandomness + new Vector3(0.1f, 0.1f, 0.1f));
    }

}
