using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(TryGetComponent<NewProp>(out NewProp other))
        {
            if(other.attributes.HasFlag(PropFlags.Metal))
            {

            }
        }
    }
}
