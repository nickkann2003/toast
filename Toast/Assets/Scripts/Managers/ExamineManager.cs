using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineManager : MonoBehaviour
{
    public static ExamineManager instance;

    [SerializeField]
    int inspectorScale = 5;

    public InspectItem inspectorItem;

    public Location examineStation; 

    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        examineStation = new Location();
        instance = this;
    }

    public void ExamineObject(Prop propToExamine)
    {
        // Right now, can only inspect basic items with a single mesh

        examineStation.cameraPos = StationManager.instance.playerLocation.cameraPos;
        examineStation.cameraRotation = StationManager.instance.playerLocation.cameraRotation;

        StationManager.instance.MoveToStation(examineStation);

        inspectorItem.inspectorCam.enabled = true;
        inspectorItem.inspecting = true;

        /*
        GameObject inspectorCopy = Instantiate(propToExamine.gameObject, inspectorItem.transform);
        if(inspectorCopy.GetComponent<Rigidbody>())
        {
            Destroy(inspectorCopy.GetComponent<Rigidbody>());
        }
        inspectorCopy.transform.position = inspectorCopy.transform.parent.position;
        inspectorCopy.layer = inspectorCopy.transform.parent.gameObject.layer;
        */

        // Default Rotation
        inspectorItem.transform.rotation = Quaternion.identity;

        // Copy model/mesh
        inspectorItem.GetComponent<MeshFilter>().mesh = propToExamine.GetComponent<MeshFilter>().mesh;
        inspectorItem.GetComponent<MeshRenderer>().material = propToExamine.GetComponent<MeshRenderer>().material;

        // Scale object properly
        inspectorItem.transform.localScale = propToExamine.transform.lossyScale * inspectorScale;
    }

    /// <summary>
    /// Hides the object examining mode
    /// </summary>
    public void QuitExamining()
    {
        inspectorItem.inspectorCam.enabled = false;
    }
}
