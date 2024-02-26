using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    public GameObject objPrefab;
    public Vector3 launchVector;
    public float launchVelocity = 400f;

    public void Use()
    {
        GameObject obj = Instantiate(objPrefab, transform.position, transform.rotation);
        obj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Use", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
