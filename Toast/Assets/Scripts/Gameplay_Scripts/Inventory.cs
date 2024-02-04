using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Inventory Class
 * ----------------------
 * Handles all item storage
 */
public class Inventory
{
    // Variables ----------------------------------------------
    private InventoryObject[] inventoryItems;

    // Constructors -------------------------------------------
    public Inventory(){
        inventoryItems = new InventoryObject[10];
    }

    public Inventory(List<GameObject> items){
        if(items.Count > 10)
        {
            inventoryItems = new InventoryObject[items.Count];
        }
        else
        {
            inventoryItems = new InventoryObject[10];
        }

        for(int i = 0; i < items.Count-1; i++)
        {
            inventoryItems[i] = new InventoryObject(items[i]);
        }
    }

    // Functions ----------------------------------------------
    public void addItem(GameObject item)
    {
        for(int i = 0;i < inventoryItems.Length;i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = new InventoryObject(item);
            }
        }
    }
    public GameObject removeItem(int position)
    {
        if (inventoryItems[position] == null)
        {
            GameObject removedObject = inventoryItems[position].GameObject;
            inventoryItems[position] = null;
            return removedObject;
        }
        return null;
    }

}

class InventoryObject
{
    // Variables ----------------------------------------------
    private GameObject gameObject;
    protected bool selectable;
    protected Sprite icon;

    public GameObject GameObject { get => gameObject; }

    // Constructors -------------------------------------------
    public InventoryObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        selectable = true;
    }

    public InventoryObject(GameObject gameObject, bool selectable)
    {
        this.gameObject = gameObject;
        this.selectable = selectable;
    }

    public InventoryObject(GameObject gameObject, Sprite icon)
    {
        this.gameObject = gameObject;
        this.icon = icon;
        selectable = true;
    }

    public InventoryObject(GameObject gameObject, bool selectable, Sprite icon)
    {
        this.gameObject = gameObject;
        this.icon = icon;
        this.selectable = selectable;
    }

    // Functions ----------------------------------------------
    public void setSelectable(bool selectable)
    {
        this.selectable = selectable;
    }
}
