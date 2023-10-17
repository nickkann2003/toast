using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnPrefabScript : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 spawnPosition;
    public Vector3 spawnRandomness;

    public Quaternion spawnRotation;
    public Quaternion spawnRotationRandomness;

    public void Start()
    {
        spawnPosition += transform.position;
    }

    public void TriggerSpawn()
    {
        Vector3 randomness = new Vector3(Random.value*spawnRandomness.x, Random.value*spawnRandomness.y, Random.value*spawnRandomness.z);
        Quaternion rotationRandomness = new Quaternion(Random.value*spawnRotationRandomness.x + spawnRotation.x, Random.value*spawnRotationRandomness.y + spawnRotation.y, Random.value*spawnRotationRandomness.z + spawnRotation.z, spawnRotation.w);
        GameObject spawnedObject = Instantiate(prefab);
        
        spawnedObject.transform.position = spawnPosition + randomness;
        spawnedObject.transform.rotation = rotationRandomness;
    }
}
