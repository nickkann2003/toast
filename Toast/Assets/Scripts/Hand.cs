using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Variables -----------------------------------------------
    public Camera cam;
    public GameObject currentItem;
    private bool holdingItem;
    public Vector3 offset;

    // Start is called before the first frame update -----------
    void Start()
    {
        holdingItem = false;
        if(currentItem != null)
        {
            holdingItem = true;
        }
    }

    // Update is called once per frame -------------------------
    void Update()
    {
        if(currentItem != null)
        {
            currentItem.transform.position = cam.transform.position + (offset);
        }
    }
}
