using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InspectItem : MonoBehaviour
{
    public bool inspecting = false;

    [SerializeField]
    Camera inspectorCam;

    Vector3 prevPosition;
    Vector3 changeInPosition;

    // Update is called once per frame
    void Update()
    {
        if (inspecting)
        {
            if(Input.GetMouseButton(0))
            {
                changeInPosition = Input.mousePosition - prevPosition;

                transform.Rotate(transform.up, -Vector3.Dot(changeInPosition, inspectorCam.transform.right), Space.World);
                transform.Rotate(inspectorCam.transform.right, Vector3.Dot(changeInPosition, inspectorCam.transform.up), Space.World);
            }

            prevPosition = Input.mousePosition;
        }
    }
}
