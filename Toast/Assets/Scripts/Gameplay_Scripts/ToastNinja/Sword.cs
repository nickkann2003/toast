using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : NewProp
{
    // ------------------------------- Variables -------------------------------
    [Header("Hand")]
    [SerializeField]
    public NewHand hand;

    //public Vector3 mOffset;
    private float mZCoord = 6;

    // ------------------------------- Functions -------------------------------
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

    /// <summary>
    ///  Gets the mouse position in world coordinates
    /// </summary>
    /// <returns>Mouse position in world coords</returns>
    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
