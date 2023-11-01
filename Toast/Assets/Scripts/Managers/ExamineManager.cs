using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineManager : MonoBehaviour
{
    public static ExamineManager instance;

    [SerializeField]
    int inspectorScale = 5;

    public InspectItem inspectorItem;

    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    public void ExamineObject(Prop propToExamine)
    {
        // Right now, can only inspect basic items with a single mesh
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
}
