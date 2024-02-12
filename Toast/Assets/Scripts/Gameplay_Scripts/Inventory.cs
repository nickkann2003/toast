using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Inventory Class
 * ----------------------
 * Handles all item storage
 */
public class Inventory : MonoBehaviour
{
    // Variables ----------------------------------------------
    [SerializeField]
    private Vector3 dropOffset = Vector3.zero;

    private List<GameObject> inventoryItems;

    // Constructors -------------------------------------------
    public Inventory(){
        inventoryItems = new List<GameObject>();
    }

    public Inventory(List<GameObject> items){
        inventoryItems = new List<GameObject>(items.Count);

        for(int i = 0; i < items.Count; i++)
        {
            inventoryItems[i] = items[i];
        }
    }

    // ----------------- MonoBehaviour Functions ------------------
    // Start is called before the first frame update -----------
    void Start()
    {

    }

    // Update is called once per frame -------------------------
    [Obsolete]
    void Update()
    {

    }

    // Functions ----------------------------------------------
    public void addItem(GameObject item)
    {
        inventoryItems.Add(item);
        item.transform.position = gameObject.transform.position + dropOffset;
    }
    public GameObject removeItem(GameObject item)
    {

        if (inventoryItems.Remove(item))
        {
            return item;
        }
        else
        {
            return null;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position + dropOffset, 0.2f);
    }

}
