using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : NewProp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (attributes.HasFlag(PropFlags.InHand))
        {
            this.transform.position = new Vector3(0,3,-5);
        }
    }
}
