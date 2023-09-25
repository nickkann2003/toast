using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<GameObject>
{
    private List<GameObject> inventoryItems;

    public Inventory<GameObject>(){
        inventoryItems = new List<GameObject>;
    }

    public Inventory<GameObject>(List<GameObject> items){
      inventoryItems = items;
    }
}
