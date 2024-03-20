using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TN_Object : MonoBehaviour, IUseStrategy
{
    [SerializeField]
    float points;
    [SerializeField]
    GameObject splatter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        GameObject obj = Instantiate(splatter, transform.position, transform.rotation);
        obj.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;

        Destroy(this.gameObject);
    }
}
