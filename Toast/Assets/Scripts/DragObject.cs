using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        if (rb != null)
        {
            // turn off gravity
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        if (rb != null)
        {
            // turn gravity back on
            rb.useGravity = true;
            rb.velocity = ((GetMouseWorldPos() + mOffset - transform.position) * 10);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        //transform.position = GetMouseWorldPos() + mOffset;
        rb.velocity = ((GetMouseWorldPos() + mOffset - transform.position) * 10);
        //rb.AddForceAtPosition(GetMouseWorldPos() + mOffset - transform.position, GetMouseWorldPos() + mOffset);
        //rb.MovePosition(GetMouseWorldPos() + mOffset);
    }
}
