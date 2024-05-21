using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class InspectItem : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("----------- Inspector Cam ------------")]
    [SerializeField]
    public Camera inspectorCam;

    private Vector3 prevPosition;
    private Vector3 changeInPosition;

    [Header("----------- Inspecting ------------")]
    public bool inspecting = false;

    // ------------------------------- Functions -------------------------------
    // Update is called once per frame
    void Update()
    {
        if (inspecting)
        {
            if(Input.GetButton("Drag"))
            {
                changeInPosition = Input.mousePosition - prevPosition;

                transform.Rotate(Vector3.up, -Vector3.Dot(changeInPosition, inspectorCam.transform.right), Space.World);
                transform.Rotate(inspectorCam.transform.right, Vector3.Dot(changeInPosition, inspectorCam.transform.up), Space.World);
            }

            prevPosition = Input.mousePosition;
        }
    }
}
