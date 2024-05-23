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

    // ------------------------------- Functions -------------------------------
    public void Start()
    {
        actualSpawnPos = spawnPosition + transform.position;
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
    /// Gizmos, shows red square for position with randomness
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spawnPosition + transform.position, spawnRandomness + new Vector3(0.1f, 0.1f, 0.1f));
    }

}
