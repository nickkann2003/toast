using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveObject : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Spawn Variables")]
    public Vector3 spawnLocation;
    public Quaternion spawnRotation;

    [Header("Persistant Reference")]
    public GameObject reference;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnLocation == Vector3.zero){
            spawnLocation = reference.transform.position;
        }
        if(spawnRotation == Quaternion.identity){
            spawnRotation = reference.transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    * Public CheckObjectiveObject
    *
    * Checks if the objective object has been destroyed
    * Called on a delay to account for ordering issues
    */
    public void CheckObjectiveObject(){
       Invoke("RefreshObject", 1);
    }

    /*
    * Private RefreshObject
    *
    * Checks if the required object is null
    * If it is, recreates it at the specified position
    */
    private bool RefreshObject(){
        if(reference == null){
            reference = Instantiate(prefab);
            reference.transform.position = spawnLocation;
            return true;
        }else{
            return false;
        }
    }
}
