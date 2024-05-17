using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class Dial : MonoBehaviour
{
    bool isDirty;

    private float mZCoord;
    private bool mouse;
    private Vector3 pos;

    /* CHANGE LATER??? */
    //public Vector3 localRotationPlane = new Vector3(0,0,1);

    // allows objects to be given parents without having a parent
    private Transform parent;

    [Header("------------- Dial Values ------------")]
    [MinMaxSlider(0, 1), Label("Min/Max Value")]
    public Vector2 minMaxValue = new Vector2 (.2f, .6f);
    [ReadOnly]
    public float dialValue;

    float snapDegrees;

    public float MinValue
    {
        get { return minMaxValue.x; }
    }
    public float MaxValue
    {
        get { return minMaxValue.y; }
    }

    [Header("------------ Dial Position Values -------------")]
    public Vector3 normal = new Vector3(0, 1.0f, 0);
    // maybe change to not be a 
    [Range(1, 180)]
    public float maxRotation = 110;

    [Header("------------- Unity Events ------------")]
    public FloatEvent onDialChange;

    
    [Header("Freeze Dial")]
    public bool freeze = false;



    private void Start()
    {
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }

        isDirty = true;
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
        if (isDirty)
        {

        }
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

            Vector3 rotation;
            rotation = transform.localEulerAngles;
            rotation.x = 0;
            rotation.y = 0;

            // if the rotation is 
            if (rotation.z > 180)
            {
                rotation.z = rotation.z - 360;
            }
            if (Mathf.Abs(rotation.z) > maxRotation)
            {
                rotation.z = maxRotation * rotation.z / Mathf.Abs(rotation.z);
            }
            dialValue = (rotation.z + maxRotation) / (maxRotation * 2);

            dialValue = dialValue * (MaxValue - MinValue) + MinValue;
            onDialChange.Invoke(dialValue);

            transform.localEulerAngles = rotation;
        }
    }

    private void UpdateDialValue()
    {

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
