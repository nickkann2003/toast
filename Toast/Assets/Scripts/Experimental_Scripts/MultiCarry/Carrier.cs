using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for things like plates, pots, etc. cabable of carrying multiple things while held
public class Carrier : MonoBehaviour
{
    // The area that detects what objects can be held
    [SerializeField]
    private CarryVolume carryVolume;
    public CarryVolume CarryVolume { get { return carryVolume;} }

    public bool isHeld;

    // The maximum amount of objects that can be carried
    [SerializeField]
    private int maxCarry;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        carryVolume.TriggerPickup();
        //isHeld = true;
        //foreach(GameObject obj in carryVolume.currentCarries)
        //{
        //    Debug.Log("Picking Up");
        //    obj.transform.SetParent(this.gameObject.transform); 
        //}
    }

    public void PutDown()
    {
        carryVolume.TriggerDrop();
       //isHeld = false;
       //foreach(GameObject obj in carryVolume.currentCarries)
       //{
       //    obj.transform.parent = null;
       //}
    }
}
