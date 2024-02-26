using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Station InventoryStation;
    public Inventory inventory;
    public Image transitionMask;

    public bool atInventory = false;

    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleInventory()
    {
        if(atInventory)
        {
            LeaveInventory();
        }
        else
        {
            ViewInventory();
        }
    }

    public void ViewInventory()
    {
        atInventory = true;
        StationManager.instance.MoveToStation(InventoryStation);
    }

    public void SetLeaveInventoryValues()
    {
        atInventory = false;
    }

    public void LeaveInventory()
    {
        SetLeaveInventoryValues();
        StationManager.instance.StationMoveBack();
    }

    public void AddItemToInventory(GameObject item)
    {

    }

    public void RemoveItemFromInventory(GameObject item)
    {

    }
}
