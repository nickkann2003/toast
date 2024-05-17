using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;

public class Dial : MonoBehaviour
{
    private float mZCoord;
    private bool mouse;
    private Vector3 pos;
    private Vector3 rotation;

    /* CHANGE LATER??? */
    //public Vector3 localRotationPlane = new Vector3(0,0,1);

    // allows objects to be given parents without having a parent
    private Transform parent;

    [Header("------------- Dial Values ------------")]
    [ReadOnly]
    public float dialValue;
    public float maxValue = .6f;
    public float minValue = .2f;

    [Header("------------ Dial Position Values -------------")]
    public Vector3 normal = new Vector3(0, 1, 0);
    public Vector3 minRotation = new Vector3(0,0,110);
    public Vector3 maxRotation = new Vector3(0, 0, 220);

    [Header("------------- Unity Events ------------")]
    public FloatEvent onDialChange;

    
    [Header("Freeze Dial")]
    public bool freeze = false;



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

            // if the rotation is 
            if (rotation.z > 180)
            {
                rotation.z = rotation.z - 360;
            }
            if (Mathf.Abs(rotation.z) > 110)
            {
                rotation.z = 110 * rotation.z / Mathf.Abs(rotation.z);
            }
            dialValue = (((rotation.z + 110) / (110f * 2)));

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
