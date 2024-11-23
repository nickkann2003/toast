using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Singleton class created to hold all PIE events for ease of access in other scripts without needing references
/// </summary>
public class PieManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public PropIntGameEvent BurnObject;
    public PropIntGameEvent CompleteMinigame;
    public PropIntGameEvent CreateObject;
    public PropIntGameEvent DestroyObjects;
    public PropIntGameEvent DragObject;
    public PropIntGameEvent DropObject;
    public PropIntGameEvent EatObject;
    public PropIntGameEvent GiveLittleFella;
    public PropIntGameEvent HaveObject;
    public PropIntGameEvent HaveObjectInInventory;
    public PropIntGameEvent PickUpObject;
    public PropIntGameEvent SetObjectOnFire;
    public PropIntGameEvent ThawObject;
    public PropIntGameEvent ToastNinjaScore;
    public PropIntGameEvent ToastObject;
    public PropIntGameEvent UseObject;
    public PropIntGameEvent FreezeObject;
    public PropIntGameEvent ToastStrength1;
    public PropIntGameEvent ToastStrength2;
    public PropIntGameEvent ToastStrength3;
    public PropIntGameEvent ToastStrength4;
    public PropIntGameEvent ToastStrength5;
    public PropIntGameEvent CapObject;
    public PropIntGameEvent UncapObject;
    public PropIntGameEvent ElectricityExplodeObject;
    public PropIntGameEvent SpreadObject;
    public PropIntGameEvent HoverObject;
    public PropIntGameEvent StopHover;
    public PropIntGameEvent ViewAchievements;

    // Singleton
    public static PieManager instance;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
    }

}
