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

    public List<GameObject> currentCarries;

    public bool isHeld;

    // The maximum amount of objects that can be carried
    [SerializeField]
    private int maxCarry;

    // Start is called before the first frame update
    void Start()
    {
        currentCarries = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        isHeld = true;
        foreach(GameObject obj in currentCarries)
        {
            Debug.Log("Picking Up");
            obj.transform.SetParent(this.gameObject.transform); 
        }
    }

    public void PutDown()
    {
        isHeld = false;

        foreach(GameObject obj in currentCarries)
        {
            obj.transform.parent = null;
        }
    }
}
