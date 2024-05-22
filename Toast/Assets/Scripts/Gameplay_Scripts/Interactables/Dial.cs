using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;

public class Dial : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    private bool isDirty;

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

    [MinValue(1), Label("Snap Points")]
    public int numSnapPoints = 2;
    [SerializeField]
    float snapAngle = 5.0f;
    float[] snapPoints;

    [Header("------------ Dial Position Values -------------")]
    [Dropdown("GetRotVector")]
    public Vector3 rotateAround;

    private DropdownList<Vector3> GetRotVector()
    {
        return new DropdownList<Vector3>()
        {
            { "X Axis (DOES NOT WORK)",   transform.parent.right },
            { "Y Axis (REALLY DOES NOT WORK)",    transform.parent.up },
            { "Z Axis",    transform.parent.forward }
        };
    }

    // maybe change to not be a single float value
    [Range(1, 180)]
    public float maxRotation = 110;

    [Header("------------- Unity Events ------------")]
    public FloatEvent onDialChange;

    
    [Header("Freeze Dial")]
    public bool freeze = false;

    // ------------------------------- Properties -------------------------------
    public float MinValue
    {
        get { return minMaxValue.x; }
    }
    public float MaxValue
    {
        get { return minMaxValue.y; }
    }

    // ------------------------------- Functions -------------------------------
    private void Start()
    {
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }

        SetSnapPoints();

        isDirty = true;
    }

    // Sets the snap points of the dial
    private void SetSnapPoints()
    {
        snapPoints = new float[numSnapPoints];

        snapPoints[0] = -110;
        float angle = maxRotation * 2 / (numSnapPoints - 1);

        for (int i = 1; i < numSnapPoints; i++)
        {
            snapPoints[i] = snapPoints[i - 1] + angle;
        }

        if (snapAngle > angle)
        {
            snapAngle = angle;
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
        if (isDirty)
        {

        }
    }

    // Togggles dial frozen
    public void ToggleFreeze()
    {
        freeze = !freeze;
    }

    // On click
    private void OnMouseDown()
    {
        mouse = true;

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

    }

    // On click release
    private void OnMouseUp()
    {
        mouse = false;
    }

    // On drag, perform calculations
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
            //else if (Mathf.Abs(maxRotation - Mathf.Abs(rotation.z)) < snapAngle)
            //{
            //    rotation.z = maxRotation * rotation.z / Mathf.Abs(rotation.z);
            //}

            for (int i = 0; i < numSnapPoints;  i++)
            {
                if (Mathf.Abs(snapPoints[i] - rotation.z) < snapAngle)
                {
                    rotation.z = snapPoints[i];
                    break;
                }
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

    // Gets the mouse world pos
    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Converts a vector3 to local pos
    private Vector3 ConvertToLocalPos(Vector3 worldPos)
    {
        Vector3 localPos = worldPos;

        if (parent != null)
        {
            localPos = parent.InverseTransformPoint(localPos);
        }

        return localPos;
    }

    // Converts a local vector to world coords
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
