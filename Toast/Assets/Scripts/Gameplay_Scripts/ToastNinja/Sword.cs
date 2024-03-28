using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : NewProp
{
    public NewHand hand;
    //public Vector3 mOffset;
    private float mZCoord = 6;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hand.transform.position = GetMouseWorldPos();
        //if (attributes.HasFlag(PropFlags.InHand))
        //{
        //    this.transform.position = new Vector3(0,3,-5);
        //}
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
