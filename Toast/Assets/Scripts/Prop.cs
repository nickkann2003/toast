using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class Prop : Station
{
    public Location currentLocation; // The location the prop is currently in

    public float toastiness = 0.0f;
    public float frozenness = 0.0f;

    private void OnMouseOver()
    {
        //if(Input.GetButtonDown("View"))
        //{
        //    ExamineManager.instance.ExamineObject(this);
        //}
    }
}
