using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastingBreadTest : MonoBehaviour
{
    private List<GameObject> collidingObjects = new List<GameObject>();
    private float targetStrength = 1;
    public Color weakestStrength = Color.white;
    public Color strongestStrength = Color.black;
    private bool isActive = false;

    private Dictionary<GameObject, ToastingObject> toastingObjects = new Dictionary<GameObject, ToastingObject>();
    private float totalTime;

    public bool IsActive { get => isActive; }

    public void Update()
    {
        if(toastingObjects.Count > 0)
        {
            foreach(var (key, value) in toastingObjects)
            {
                ToastingObject toast = value;
                toast.adjustColor(totalTime, Time.deltaTime);
            }
        }
    }

    public void activateTrigger(float maxTime)
    {
        totalTime = maxTime;
        Color targetColor = (strongestStrength - weakestStrength) * targetStrength + weakestStrength;
        foreach (GameObject obj in collidingObjects)
        {
            if (obj != null)
            {
                Color totalOffset = targetColor - obj.GetComponent<Renderer>().material.color;
                if (!toastingObjects.ContainsKey(obj))
                {
                    toastingObjects.Add(obj, new ToastingObject(obj.GetComponent<Renderer>(), totalOffset));
                }
            }
        }
        isActive = true;
    }

    public void deactivateTrigger()
    {
        toastingObjects.Clear();
        isActive = false;
    }

    public void setDialValue(float value)
    {
        targetStrength = value;
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if(!collidingObjects.Contains(other.gameObject))
                collidingObjects.Add(other.gameObject);
        }
        catch
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        try
        {
            if (collidingObjects.Contains(other.gameObject))
                collidingObjects.Remove(other.gameObject);
            
            if (toastingObjects.ContainsKey(other.gameObject))
                toastingObjects.Remove(other.gameObject);
            
            
        }
        catch
        {
            return;
        }
    }
}

class ToastingObject {
    Renderer renderer;
    Color totalOffset;

    public ToastingObject(Renderer renderer, Color totalOffset)
    {
        this.renderer = renderer;
        this.totalOffset = totalOffset;
    }

    public void adjustColor(float maxTime, float deltaTime)
    {
        float pChange = deltaTime / maxTime;
        renderer.material.color += totalOffset*pChange;
    }
}
