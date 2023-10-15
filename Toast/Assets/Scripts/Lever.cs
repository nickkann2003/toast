using System;
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
    public List<LeverChild> children;

    private bool mouse;
    private float percent;

    private Vector3 pos;

    // allows objects to be given parents without having a parent
    private Transform parent;

    // timer
    public float timer;
    public float maxTime;

    private void Start()
    {
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }
        foreach(LeverChild child in children) {
            child.top += child.rigidBody.position;
            child.bottom += child.rigidBody.position;
        }
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (!mouse)
        {
            
            pos = ConvertToLocalPos(transform.position);

            if (pos.y < maxHeight)
            {
                pos.y += ((maxHeight - minHeight) / .2f) * Time.deltaTime;
            }

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
        if (timer <= 0)
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
                timer = maxTime;
            }

            percent = (pos.y - minHeight) / (maxHeight - minHeight);
            foreach (LeverChild child in children)
            {
                child.rigidBody.velocity = (child.bottom + (child.top - child.bottom)*percent - child.rigidBody.transform.localPosition) * 25;
            }

            // convert from local to world space
            transform.position = ConvertToWorldPos(pos);
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