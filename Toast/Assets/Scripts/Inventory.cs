using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Inventory Class
 * ----------------------
 * Handles all item storage
 */
public class Inventory
{
    // Variables ----------------------------------------------
    private List<GameObject> inventoryItems;

    // Constructors -------------------------------------------
    public Inventory(){
        inventoryItems = new List<GameObject>();
    }

    public Inventory(List<GameObject> items){
      inventoryItems = items;
    }

    // Functions ----------------------------------------------
    public void addItem(GameObject item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
        }
        else
        {
            return;
        }
    }
    public void removeItem(GameObject item)
    {
        if(inventoryItems.Contains(item))
        {
            inventoryItems.Remove(item);
        }
        else
        {
            return;
        }
    }
}
