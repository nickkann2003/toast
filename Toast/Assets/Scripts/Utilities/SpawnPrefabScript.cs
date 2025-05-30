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

    public GameObject spawnParent; // Object to set as parent when spawning

    [SerializeField]
    private bool automaticSpawning;
    [Header("----------------- Automatic Spawning -----------------"), ShowIf("automaticSpawning")]
    [SerializeField]
    private float spawnRatePerSecond;
    private float spawnDelay;
    private float cd;
    [SerializeField, ShowIf("automaticSpawning")]
    private float spawnDuration = 0f;
    private float remainingSpawnDuration;

    // ------------------------------- Functions -------------------------------
    public void Start()
    {
        actualSpawnPos = spawnPosition + transform.position;
        spawnDelay = 1f / spawnRatePerSecond;
        remainingSpawnDuration = spawnDuration;
    }

    private void Update()
    {
        if (automaticSpawning && gameObject.activeSelf)
        {
            cd -= Time.deltaTime;
            remainingSpawnDuration -= Time.deltaTime;
            if (cd < 0f)
            {
                if(spawnDuration > 0)
                {
                    if(remainingSpawnDuration > 0)
                    {
                        TriggerSpawn();
                        cd = spawnDelay;
                    }
                }
                else
                {
                    TriggerSpawn();
                    cd = spawnDelay;
                }
            }
        }
    }

    /// <summary>
    /// Triggers the spawn of this prefab
    /// </summary>
    public void TriggerSpawn()
    {
        if (prefab != null)
        {
            Vector3 randomness = new Vector3(Random.value * spawnRandomness.x, Random.value * spawnRandomness.y, Random.value * spawnRandomness.z);
            Quaternion rotationRandomness = new Quaternion(Random.value * spawnRotationRandomness.x + spawnRotation.x, Random.value * spawnRotationRandomness.y + spawnRotation.y, Random.value * spawnRotationRandomness.z + spawnRotation.z, spawnRotation.w);
            GameObject spawnedObject = Instantiate(prefab);

            spawnedObject.transform.position = actualSpawnPos + randomness;
            spawnedObject.transform.rotation = rotationRandomness;

            if (spawnParent != null)
                spawnedObject.transform.SetParent(spawnParent.transform, true);
        }
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
