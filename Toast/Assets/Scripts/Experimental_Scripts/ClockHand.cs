using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField]
    private GameObject clockBody;

    private Vector3 prevPos;

    // ------------------------------- Functions -------------------------------
    private void Start()
    {
       
    }

    private void Update()
    {
        
    }

    // On click
    private void OnMouseDown()
    {
        prevPos = Input.mousePosition;
    }

    // On click release
    private void OnMouseUp()
    {

    }

    // On drag, perform calculations
    private void OnMouseDrag()
    {
        CalcRotation();
    }

    void CalcRotation()
    {
        // Set center of clock
        Vector3 center = Camera.main.WorldToScreenPoint(clockBody.transform.position);

        // Calculate previous angle
        float anglePrevious = Mathf.Atan2(center.x - prevPos.x, prevPos.y - center.y);

        // Store current position
        Vector3 currPosition = Input.mousePosition;

        // Calculate angle to current position (in radians)
        float angleNow = Mathf.Atan2(center.x - currPosition.x, currPosition.y - center.y);

        // Update previous position
        prevPos = currPosition;

        // Calculate change in angle
        float angleDelta = angleNow - anglePrevious;

        // Convert to degrees
        angleDelta *= Mathf.Rad2Deg;

        // Rotate around
        transform.RotateAround(clockBody.transform.position, clockBody.transform.up, -angleDelta);
    }

}
