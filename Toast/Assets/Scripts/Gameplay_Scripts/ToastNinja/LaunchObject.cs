using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchObject : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Prefabs")]
    [SerializeField]
    public GameObject objPrefab;
    [SerializeField]
    ParticleSystem particles;

    [Header("Launch Values")]
    [SerializeField]
    public Vector3 launchVector;
    [SerializeField]
    public float launchVelocity = 400f;
    [SerializeField]
    public bool active = false;

    // ------------------------------- Functions -------------------------------
    /// <summary>
    /// Launchs this items prefab
    /// </summary>
    public void Launch()
    {
        LaunchObj(objPrefab);
    }

    /// <summary>
    /// Launches a given object
    /// </summary>
    /// <param name="objectToLaunch">Object to be launched</param>
    public void Launch(GameObject objectToLaunch)
    {
        LaunchObj(objectToLaunch);
    }

    /// <summary>
    /// Launches a given game object
    /// </summary>
    /// <param name="objectToLaunch">Object to be launched</param>
    private void LaunchObj(GameObject objectToLaunch)
    {
        if (!active)
        {
            return;
        }
        particles.Play();
        StartCoroutine(DelayedLaunchObj(objectToLaunch));

    }

    /// <summary>
    /// DelayedLaunch, launches an objet after a delay
    /// </summary>
    /// <param name="objectToLaunch">The object being launched</param>
    /// <returns>Coroutine</returns>
    IEnumerator DelayedLaunchObj(GameObject objectToLaunch)
    {
        yield return new WaitForSeconds(2f);

        if (!active)
        {
            yield break;
        }

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
