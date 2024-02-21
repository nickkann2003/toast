using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// in order to check to see if the prop contains a flag use HasFlag
// in order to check to see if the prop doesn't contain a flag use Equals(PropFlags.None)
[Flags]
public enum PropFlags
{
    None = 0,
    Toast = 1 << 0,
    Jammed = 1 << 1,
    Metal = 1 << 2,
    Giant = 1 << 3,
    ImmuneToFreeze = 1 << 4,
    ImmuneToToast = 1 << 5,
    InHand = 1 << 6,
    Frozen = 1 << 7,
    Bread = 1 << 29,
    Jam = 1 << 30,
    Knife = 1 << 31,
}