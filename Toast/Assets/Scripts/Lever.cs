using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Lever : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public float maxHeight;
    public float minHeight;

    //public Rigidbody rb;
    public float mouseY;
    public Vector3 localPos;

    // timer
    public float timer;
    public float maxTime;

    private void Start()
    {
        //timer = 0.0f;
        //maxTime = 60.0f;
        //rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            localPos = transform.localPosition;
            if (localPos.y < maxHeight)
            {
                localPos.y += ((maxHeight - localPos.y) / .1f) * Time.deltaTime;
                transform.localPosition = localPos;
            }
        }
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = transform.position - GetMouseWorldPos();

        mouseY = GetMouseWorldPos().y + mOffset.y;
    }

    private void OnMouseDrag()
    {
        if (timer <= 0)
        {
            // CLEAN UP THIS CODE || MOUSE IS IN WORLD SPACE BUT TRANSFORMING BASED ON LOCAL POSITION
            localPos = transform.localPosition;
            localPos.y = GetMouseWorldPos().y + mOffset.y;

            if (localPos.y > maxHeight)
            {
                localPos.y = maxHeight;
            }
            else if (localPos.y < minHeight)
            {
                localPos.y = minHeight;
                timer = maxTime;
            }

            transform.localPosition = localPos;
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
}
