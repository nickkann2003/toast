using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : NewProp
{
    // ------------------------------- Variables -------------------------------
    [Header("Hand")]
    [SerializeField]
    public NewHand hand;

    [SerializeField]
    private PhysicalButtons button;

    [SerializeField]
    private GameObject container;

    private bool sheathed = true;

    //public Vector3 mOffset;
    private float mZCoord = 6;

    //[SerializeField]
    //private ToastNinja toastNinja;

    [SerializeField]
    private GameObject blade;

    [SerializeField]
    private Vector3 unsheathedRot;

    [SerializeField]
    private GameObject sheathedPos;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
        //container.transform.position = GetMouseWorldPos();
        if (propFlags.HasFlag(PropFlags.InHand))
        {
            if (sheathed)
            {
                // start toast ninja
                button.ForceActivate();
                if (blade != null)
                {
                    this.GetComponent<BoxCollider>().enabled = false;
                    blade.SetActive(true);
                }

                sheathed = false;
            }
            container.transform.localEulerAngles = unsheathedRot;
            container.transform.position = GetMouseWorldPos();
        }
        else
        {
            if (!sheathed)
            {
                // stop toast ninja
                button.ForceActivate();
                if (blade != null)
                {
                    blade.SetActive(false);
                    this.GetComponent<BoxCollider>().enabled = true;

                    container.transform.localPosition = Vector3.zero;
                    transform.parent = sheathedPos.transform;
                    transform.localPosition = Vector3.zero;
                    transform.localEulerAngles = Vector3.zero;
                }
                sheathed = true;
            }
        }
    }

    /// <summary>
    ///  Gets the mouse position in world coordinates
    /// </summary>
    /// <returns>Mouse position in world coords</returns>
    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
