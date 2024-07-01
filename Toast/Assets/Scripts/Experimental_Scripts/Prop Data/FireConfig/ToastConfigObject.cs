using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prop Toast Config", menuName = "Prop/Config/Toast", order = 53)]
public class ToastConfigObject : ScriptableObject
{
    [SerializeField, CurveRange(0,0,1,1)]
    private AnimationCurve toastCurve;
    [SerializeField]
    private float toastTrigger;
    [SerializeField]
    private float fireTrigger;

    //private Dictionary<float, ToastTrigger>

    //public void Toast(NewProp newProp, float val)
    //{

    //}
}

public class ToastTrigger
{
    [SerializeField]
    private float breakpoint;

    public float Breakpoint { get { return breakpoint; } }
}
