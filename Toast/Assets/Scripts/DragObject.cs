using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObjects : MonoBehaviour, IHighlightable
{
    private Vector3 mOffset;
    private float mZCoord;
    
    // Hand
    private Hand hand;

    public Rigidbody rb;

    private Outline outline;
    private bool isDragging;
    private bool isEnable; // this is used to enable/disable the interaction of the objects

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hand = Camera.main.GetComponent<Hand>();

        outline = GetComponent<Outline>();
        isEnable= true;
    }

    private void OnMouseDown()
    {
        
        if (rb != null)
        {
            // On pick up, if the grabbed item is the currently held item, remove it from hand
            if (hand.CheckHeldItem(rb.gameObject))
            {
                hand.RemoveItem(mZCoord);
            }
            // turn off gravity
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = transform.position - GetMouseWorldPos();

        // Outline
        outline.enabled = false;
        isDragging = true;
    }

    private void OnMouseUp()
    {
        if (rb != null)
        {
            // If released in pick-up zone for hand
            if (hand.CheckDrop())
            {
                // Add the item to hand
                hand.AddItem(rb.gameObject);
            }
            else
            {
                // turn gravity back on
                rb.useGravity = true;
                rb.velocity = ((GetMouseWorldPos() + mOffset - transform.position) * 10);
            }
        }

        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coord of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        //transform.position = GetMouseWorldPos() + mOffset;
        rb.velocity = ((GetMouseWorldPos() + mOffset - transform.position) * 10);
        //rb.AddForceAtPosition(GetMouseWorldPos() + mOffset - transform.position, GetMouseWorldPos() + mOffset);
        //rb.MovePosition(GetMouseWorldPos() + mOffset);

    }


    
    private void OnMouseOver()
    {
        if (!isDragging && isEnable)
            EnableHiglight();


    }

    private void OnMouseExit()
    {
        if (!isDragging && isEnable)
           DisableHighlight();
    }

    public void EnableHiglight()
    {
        outline.enabled = true;
        GameManager.Instance.SetHandCursor();
    }

    public void DisableHighlight()
    {
        outline.enabled = false;
        GameManager.Instance.SetDefaultCursor();
    }

    private void Update()
    {
    }
}
