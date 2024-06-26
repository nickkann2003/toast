using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ExamineManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Instance")]
    public static ExamineManager instance;

    [Header("Examine Variables")]
    [SerializeField]
    int inspectorScale = 5;
    public InspectItem inspectorItem;
    public bool freezeOnExamine;

    [Header("References")]
    public Station examineStation;
    [SerializeField] private VolumeProfile blurProfile;
    [SerializeField] private VolumeProfile globalProfile;
    [SerializeField] private Volume globalVolume;
    [SerializeField] public Raycast raycast; // Temp
    [SerializeField] Canvas background;

    private GameObject currentExamine;

    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //backgroundBlur.profile.TryGet<DepthOfField>(out dof);

        //dof.mode.value = DepthOfFieldMode.Off;

        background.gameObject.SetActive(false);

    }

    /// <summary>
    /// Sets the game into examine mode on a specific object
    /// </summary>
    /// <param name="propToExamine">The prop to be examined</param>
    public void ExamineObject(GameObject propToExamine)
    {
        // Ensure can't have more than one item being inspected
        if(currentExamine != null)
        {
            return;
        }
        if (freezeOnExamine)
        {
            Time.timeScale = 0;
        }

        // Right now, can only inspect basic items with a single mesh

        examineStation.cameraPos = StationManager.instance.playerLocation.cameraPos;
        examineStation.cameraRotation = StationManager.instance.playerLocation.cameraRotation;

        StationManager.instance.MoveToStation(examineStation);

        inspectorItem.inspectorCam.enabled = true;
        inspectorItem.inspecting = true;

        //dof.mode.value = DepthOfFieldMode.Gaussian;
        globalVolume.profile = blurProfile;

          // Create copy of object
          currentExamine = Instantiate(propToExamine, inspectorItem.transform);

        // Remove components that may alter behavior
        if(currentExamine.GetComponent<Rigidbody>())
        {
            Destroy(currentExamine.GetComponent<Rigidbody>());
            //Destroy(currentExamine.GetComponent<DragObjects>());
        }

        currentExamine.transform.position = currentExamine.transform.parent.position;
        currentExamine.layer = currentExamine.transform.parent.gameObject.layer;

        // Default Rotation
        currentExamine.transform.rotation = Quaternion.identity;

        // Copy model/mesh
        //inspectorItem.GetComponent<MeshFilter>().mesh = propToExamine.GetComponent<MeshFilter>().mesh;
        //inspectorItem.GetComponent<MeshRenderer>().material = propToExamine.GetComponent<MeshRenderer>().material;

        // Scale object properly
        currentExamine.transform.localScale = propToExamine.transform.localScale * inspectorScale;

        background.gameObject.SetActive(true);
        raycast.enabled = false;
    }

    /// <summary>
    /// Hides the object examining mode
    /// </summary>
    public void QuitExamining()
    {
        inspectorItem.inspectorCam.enabled = false;

        // dof.mode.value = DepthOfFieldMode.Off;
        globalVolume.profile = globalProfile;

        Destroy(currentExamine);

        if (freezeOnExamine)
        {
            Time.timeScale = 1;
        }

        background.gameObject.SetActive(false);
        raycast.enabled = true;
    }
}
