using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

public class Prop : Station
{
    public Location currentLocation; // The location the prop is currently in

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ExamineManager.instance.ExamineObject(this);
        }    
    }
}
