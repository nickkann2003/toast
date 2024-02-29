using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class Lever : MonoBehaviour
{
    [SerializeField] private UnityEvent leverTrigger;

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

    // Has this reached bottom yet?
    private bool isOn;

    // amount that the button has moved towards min height
    [SerializeField]
    private float baseSpeedInterpolation = 1;
    [SerializeField]
    private float maxSpeedInterpolation = 8;
    private float interpolateAmount;
    private float interpolationSpeed;

    private void Awake()
    {
        isOn = false;
    }
    private void Start()
    {
        mouse = false;

        if (transform.parent != null && parent == null)
        {
            parent = transform.parent;
        }
        //foreach(LeverChild child in children) {
        //    child.top += child.rigidBody.position;
        //    child.bottom += child.rigidBody.position;
        //}
    }

    private void Update()
    {
        if (isOn)
        {
            //timer -= Time.deltaTime;
            percent = (pos.y - minHeight) / (maxHeight - minHeight);
            foreach (LeverChild child in children)
            {
                child.rigidBody.velocity = (child.bottom + ((child.top - child.bottom) * percent) - child.rigidBody.transform.localPosition) * 25;
            }
        }
        else if (!mouse)
        {
            
            pos = ConvertToLocalPos(transform.position);

            if (interpolateAmount > 0 && interpolateAmount <= 1)
            {
                interpolateAmount -= Time.deltaTime * interpolationSpeed;
            }
            else
            {
                interpolateAmount = 0;
            }

            pos = Vector3.Lerp(new Vector3(pos.x, maxHeight, pos.z), new Vector3(pos.x, minHeight, pos.z), interpolateAmount);

            //if (pos.y < maxHeight)
            //{
            //    pos.y += ((maxHeight - minHeight) / .2f) * Time.deltaTime;
            //}
            //else if (pos.y > maxHeight)
            //{
            //    pos.y = maxHeight;
            //}

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
        interpolateAmount = 1 - (ConvertToLocalPos(transform.position).y - minHeight) / (maxHeight - minHeight);
        interpolationSpeed = (maxSpeedInterpolation - baseSpeedInterpolation) * interpolateAmount + baseSpeedInterpolation;
        mouse = false;
    }

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
                child.rigidBody.velocity = (child.bottom + (child.top - child.bottom)*percent - child.rigidBody.transform.localPosition) * 25;
            }

            // convert from local to world space
            transform.position = ConvertToWorldPos(pos);
        }
    }

    public void TurnOn()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterLever);
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterTimer);
        isOn = true;
    }

    public void TurnOff()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterPop);
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.toasterDing);
        isOn = false;
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