using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public static InventoryManager instance;

    public GameObject inventoryButton;
    public Station InventoryStation;
    public Inventory inventory;
    public Image transitionMask;

    public bool atInventory = false;

    public bool HoveringInventory { get => hoveringInventory; }
    private bool hoveringInventory = false;

    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(inventoryButton.GetComponent<EventTrigger>() == null) 
        { 
            inventoryButton.AddComponent<EventTrigger>();
        }
        EventTrigger trigger = inventoryButton.GetComponent<EventTrigger>();
        
        EventTrigger.Entry inEvent = new EventTrigger.Entry();
        inEvent.eventID = EventTriggerType.PointerEnter;
        EventTrigger.Entry outEvent = new EventTrigger.Entry();
        outEvent.eventID = EventTriggerType.PointerExit;

        inEvent.callback.AddListener(SetHoverInventoryTrue);
        outEvent.callback.AddListener(SetHoverInventoryFalse);
        
        trigger.triggers.Add(inEvent);
        trigger.triggers.Add(outEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Toggles going to/from inventory
    /// </summary>
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

    /// <summary>
    /// Moves player to inventory station
    /// </summary>
    public void ViewInventory()
    {
        atInventory = true;
        StationManager.instance.MoveToStation(InventoryStation);
    }

    /// <summary>
    /// Performs all value changes necessary for leaving the inventory
    /// </summary>
    public void SetLeaveInventoryValues()
    {
        atInventory = false;
    }

    /// <summary>
    /// Leaves the inventory station and moves back one station
    /// </summary>
    public void LeaveInventory()
    {
        SetLeaveInventoryValues();
        StationManager.instance.StationMoveBack();
    }

    /// <summary>
    /// Adds a given item to the inventory, performs objective checks
    /// </summary>
    /// <param name="item">Item to be added</param>
    public void AddItemToInventory(GameObject item)
    {
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.HaveObjectsInInventory, item.GetComponent<NewProp>().attributes, true));
        item.transform.position = InventoryStation.objectOffset;
    }

    /// <summary>
    /// Removes an item from the inventory, performs objective checks
    /// </summary>
    /// <param name="item">Item to be removed</param>
    public void RemoveItemFromInventory(GameObject item)
    {
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.HaveObjectsInInventory, item.GetComponent<NewProp>().attributes, false));
        if (atInventory)
        {
            // TODO this is almost code!
            Station temp = StationManager.instance.playerPath.Pop();
            item.transform.position = StationManager.instance.playerPath.Peek().objectOffset;
            StationManager.instance.playerPath.Push(temp);
        }
    }

    /// <summary>
    /// Sets hovering inventory values to true
    /// </summary>
    /// <param name="eventData"></param>
    private void SetHoverInventoryTrue(BaseEventData eventData)
    {
        hoveringInventory = true;
    }

    /// <summary>
    /// Sets hovering inventory values to false
    /// </summary>
    /// <param name="eventData"></param>
    private void SetHoverInventoryFalse(BaseEventData eventData)
    {
        hoveringInventory = false;
    }
}
