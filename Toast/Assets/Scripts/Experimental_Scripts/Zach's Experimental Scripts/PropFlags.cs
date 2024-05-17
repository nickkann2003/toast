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
    OnFire = 1 << 8,
    Burnt = 1 << 9,
    ImmuneToPickup = 1 << 10,
    ImmuneToDrag = 1 << 11,
    ImmuneToDrop = 1 << 12,
    Bread = 1 << 28,
    Jam = 1 << 29,
    Knife = 1 << 30,
    JamLid = 1 << 31
}