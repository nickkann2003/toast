using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class Lever : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("---------- Unity Events -----------")]
    [SerializeField] private UnityEvent leverTrigger;

    private Vector3 mOffset;
    private float mZCoord;

    [Header("------------- Lever Variables ------------")]
    public float maxHeight;
    public float minHeight;
    public List<LeverChild> children;

    private bool mouse;
    private float percent;

    private Vector3 pos;

    // allows objects to be given parents without having a parent
    private Transform parent;

    // Has this reached bottom yet?
    private bool isOn;

    // amount that the button has moved towards min height
    [SerializeField]
    private float baseSpeedInterpolation = 1;
    [SerializeField]
    private float maxSpeedInterpolation = 8;
    private float interpolateAmount;
    private float interpolationSpeed;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        isOn = false;
    }

    // On start, set parent
    private void Start()
    {
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }
    }

    // Update each frame
    private void Update()
    {
        // If on, set percent and positions
        if (isOn)
        {
            percent = (pos.y - minHeight) / (maxHeight - minHeight);
            foreach (LeverChild child in children)
            {
                child.rigidBody.velocity = (child.bottom + ((child.top - child.bottom) * percent) - child.rigidBody.transform.localPosition) * 25;
            }
        }
        else if (!mouse)
        {
            // If not on, and not clicking

            // Convert world to local pos
            pos = ConvertToLocalPos(transform.position);

            // If interpolating, calculate
            if (interpolateAmount > 0 && interpolateAmount <= 1)
            {
                interpolateAmount -= Time.deltaTime * interpolationSpeed;
            }
            else
            {
                interpolateAmount = 0;
            }

            // Lerp from current pos to new location
            pos = Vector3.Lerp(new Vector3(pos.x, maxHeight, pos.z), new Vector3(pos.x, minHeight, pos.z), interpolateAmount);

            //Set all children to correct %
            percent = (pos.y - minHeight) / (maxHeight - minHeight);
            foreach (LeverChild child in children)
            {
                child.rigidBody.velocity = (child.bottom + ((child.top - child.bottom) * percent) - child.rigidBody.transform.localPosition) * 25;
            }

            // convert from local to world pos if object has parent
            pos = ConvertToWorldPos(pos);

            transform.position = pos;
        }
    }

    // On mouse down, set values
    private void OnMouseDown()
    {
        mouse = true;

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = transform.position - GetMouseWorldPos();
    }

    // On mouse up, clear values
    private void OnMouseUp()
    {
        interpolateAmount = 1 - (ConvertToLocalPos(transform.position).y - minHeight) / (maxHeight - minHeight);
        interpolationSpeed = (maxSpeedInterpolation - baseSpeedInterpolation) * interpolateAmount + baseSpeedInterpolation;
        mouse = false;
    }

    // On drag, cap at ends and calculate percent
    private void OnMouseDrag()
    {
        if (!isOn)
        {
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            // get the button position in world space
            pos = transform.position;
            // set the y pos as the pos of the mouse
            pos.y = GetMouseWorldPos().y + mOffset.y;
            // convert from world to local space
            pos = ConvertToLocalPos(pos);

            if (pos.y > maxHeight)
            {
                pos.y = maxHeight;
            }
            else if (pos.y < minHeight)
            {
                pos.y = minHeight;

                if (!isOn)
                {
                    leverTrigger.Invoke();
                }
            }

            percent = (pos.y - minHeight) / (maxHeight - minHeight);

            foreach (LeverChild child in children)
            {
                child.rigidBody.velocity = (child.bottom + (child.top - child.bottom) * percent - child.rigidBody.transform.localPosition) * 25;
            }

            // convert from local to world space
            transform.position = ConvertToWorldPos(pos);
        }
    }

    // Turns this lever on
    public void TurnOn()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterLever);
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterTimer);
        isOn = true;
    }

    // Turns this lever off
    public void TurnOff()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterPop);
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterDing);
        isOn = false;
    }

    // Gets mouse pos in world coords
    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Converts world coords to local coords
    private Vector3 ConvertToLocalPos(Vector3 worldPos)
    {
        Vector3 localPos = worldPos;

        if (parent != null)
        {
            localPos = parent.InverseTransformPoint(localPos);
        }

        return localPos;
    }

    // Converts local coords to world coords
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

[System.Serializable]
public class LeverChild
{
    public Rigidbody rigidBody;
    public Vector3 top;
    public Vector3 bottom;

    public LeverChild(Rigidbody rigidBody, Vector3 top, Vector3 bottom)
    {
        this.rigidBody = rigidBody;
        this.top = top;
        this.bottom = bottom;
    }
}