using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;

[RequireComponent(typeof(Highlights))]
public class Prop : MonoBehaviour
{
    public Station currentLocation; // The location the prop is currently in

    private Highlights highlight;
    public float toastiness = 0.0f;
    public float frozenness = 0.0f;
}
