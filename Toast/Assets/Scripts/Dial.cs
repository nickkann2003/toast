using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    public Vector3 mOffset;
    private float mZCoord;

    private bool mouse;

    private Vector3 pos;
    public Vector3 rotation;

    // allows objects to be given parents without having a parent
    private Transform parent;
    public Vector3 lookAtPos;

    // timer
    public float timer;
    public float maxTime;

    private void Start()
    {
        rotation = transform.eulerAngles;
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }
    }

    private void Update()
    {
        //if (timer > 0)
        //{
        //    timer -= Time.deltaTime;
        //}
        //else if (!mouse)
        //{

        //    pos = ConvertToLocalPos(transform.position);

        //    if (pos.y < maxHeight)
        //    {
        //        pos.y += ((maxHeight - minHeight) / .1f) * Time.deltaTime;
        //    }
        //    // convert from local to world pos if object has parent
        //    pos = ConvertToWorldPos(pos);

        //    transform.position = pos;
        //}
    }

    private void OnMouseDown()
    {
        mouse = true;

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        mouse = false;
    }

    private void OnMouseDrag()
    {
        transform.up = GetMouseWorldPos() - transform.position;
        
        rotation = transform.eulerAngles;
        if (transform.eulerAngles.z > 110 && transform.eulerAngles.z <= 180)
        {
            rotation.z = 110f;
            transform.eulerAngles = rotation;
        }
        else if ((transform.eulerAngles.z > 180 && transform.eulerAngles.z < 250))
        {
            rotation.z = 250f;
            transform.eulerAngles = rotation;
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

    private Vector3 ConvertToLocalPos(Vector3 worldPos)
    {
        Vector3 localPos = worldPos;

        if (parent != null)
        {
            localPos = parent.InverseTransformPoint(localPos);
        }

        return localPos;
    }

    private Vector3 ConvertToWorldPos(Vector3 localPos)
    {
        Vector3 worldPos = localPos;

        if (parent != null)
        {
            worldPos = parent.TransformPoint(worldPos);
        }

        return worldPos;
    }
}