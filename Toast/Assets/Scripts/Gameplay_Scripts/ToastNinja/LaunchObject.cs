using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    public GameObject objPrefab;
    public Vector3 launchVector;
    public float launchVelocity = 400f;

    [SerializeField]
    ParticleSystem particles;

    public void Launch()
    {
        LaunchObj(objPrefab);
    }

    public void Launch(GameObject objectToLaunch)
    {
        LaunchObj(objectToLaunch);
    }

    private void LaunchObj(GameObject objectToLaunch)
    {
        particles.Play();
        StartCoroutine(DelayedLaunchObj(objectToLaunch));

    }

    IEnumerator DelayedLaunchObj(GameObject objectToLaunch)
    {
        yield return new WaitForSeconds(2f);

        GameObject obj = Instantiate(objectToLaunch, transform.position, transform.rotation);
        obj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, launchVelocity, 0));
        if (transform.rotation.z == 0)
        {
            obj.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, Random.Range(-10, 10)));
        }
        else
        {
            obj.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, Random.Range(3, 15) * (transform.rotation.z / Mathf.Abs(transform.rotation.z))));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Use", 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
