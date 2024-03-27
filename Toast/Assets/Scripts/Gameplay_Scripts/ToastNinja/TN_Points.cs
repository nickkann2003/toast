using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Points : MonoBehaviour
{
    [SerializeField]
    private float timer;
    [SerializeField]
    private Vector3 vel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }

        transform.Translate(vel * Time.deltaTime);

        timer -= Time.deltaTime;
    }
}
