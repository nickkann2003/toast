using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool freeze = false;

    public float dialValue;
    public float maxValue = .6f;
    public float minValue = .2f;

    public FloatEvent onDialChange;


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

    public void ToggleFreeze()
    {
        freeze = !freeze;
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
        if (!freeze)
        {
            transform.up = ConvertToLocalPos(GetMouseWorldPos()) - transform.localPosition;
            
            rotation = transform.localEulerAngles;
            rotation.x = 0;
            rotation.y = 0;
            if (transform.localEulerAngles.z > 110 && transform.localEulerAngles.z <= 180)
            {
                rotation.z = 110f;
            }
            else if ((transform.localEulerAngles.z > 180 && transform.localEulerAngles.z < 250))
            {
                rotation.z = 250f;
            }
            if (rotation.z >= 250)
            {
                dialValue = (1 - ((rotation.z - 250f) / 110f)) * 0.5f + 0.5f;
            }
            else if (rotation.z <= 110)
            {
                dialValue = (1 - ((rotation.z) / 110f)) * 0.5f;
            }

            dialValue = dialValue * (maxValue - minValue) + minValue;
            onDialChange.Invoke(dialValue);

            transform.localEulerAngles = rotation;
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
