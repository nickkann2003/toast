using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Highlights))]
public class Station : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("------------ Transform Variables ------------")]
    public Vector3 cameraPos;
    public Quaternion cameraRotation = Quaternion.identity;

    [Header("------------ Obj Offset ------------")]
    public Vector3 objectOffset;

    [Header("------------ Station Variables ------------")]
    public Station parentLoc;
    public Stations stationLabel;

    public Collider clickableCollider;
    private Collider[] myClickableColliders;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    private Highlights highlight;

    [Header("------------ Enable Station ------------")]
    [SerializeField] private bool isEnabled;

    [Header("------------ Drag Planes ------------")]
    public List<GameObject> dragPlanes;
    //public GameObject dragPlane;

    [Header("------------ Station Specific Objects -------------")]
    public List<GameObject> stationSpecificHints = new List<GameObject>();

    [Header("------------ Events ------------")]
    [SerializeField] private UnityEvent arrive;
    [SerializeField] private UnityEvent leave;

    [SerializeField, Button]
    private void SetCameraPositionAndRotation() 
    {
        cameraPos = transform.InverseTransformPoint(SceneView.GetAllSceneCameras()[0].transform.position);
        cameraRotation = Quaternion.Euler(transform.InverseTransformDirection(SceneView.GetAllSceneCameras()[0].transform.eulerAngles));
    }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        isEnabled = true;
        myClickableColliders = GetComponents<Collider>();
        if (clickableCollider == null)
        {
            clickableCollider = GetComponent<Collider>();
        }

        foreach (GameObject obj in stationSpecificHints)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// On mouse down, call station manager to visit this station
    /// </summary>
    private void OnMouseDown()
    {
        if(this == StationManager.instance.playerLocation.parentLoc)
        {
            return;
        }

        if (StationManager.instance != null && StationManager.instance.playerLocation != null)
        {
            if (StationManager.instance.playerLocation.interactables != null)
            {
                if (StationManager.instance.playerLocation.interactables.Contains(this))
                {
                    StationManager.instance.MoveToStation(this);
                }
            }
        }
    }

    /// <summary>
    /// Calls necessary arrival functions for this station
    /// </summary>
    public void OnArrive()
    {
        arrive.Invoke();
        foreach(GameObject obj in stationSpecificHints)
        {
            obj.SetActive(true);
        }
    }
    
    /// <summary>
    /// Calls necessary Leave functions for this station
    /// </summary>
    public void OnLeave()
    {
        leave.Invoke();
        foreach (GameObject obj in stationSpecificHints)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// Gizmos:
    /// Camera orb with FOV view
    /// Blue wire sphere for drop location
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.TransformPoint(objectOffset), 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(cameraPos), 0.1f);
        Matrix4x4 matrix = Matrix4x4.Translate(transform.TransformPoint(cameraPos)) * Matrix4x4.Rotate(Quaternion.Euler(transform.TransformDirection(cameraRotation.eulerAngles)));
        Gizmos.matrix = matrix;
        Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);

    }

    /// <summary>
    /// Disables attached dragplanes
    /// </summary>
    public void DisableDragPlanes()
    {
        if (dragPlanes.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < dragPlanes.Count; i++)
        {
            dragPlanes[i].SetActive(false);
        }
    }

    /// <summary>
    /// Enables attached dragplanes
    /// </summary>
    public void EnableDragPlanes()
    {
        if (dragPlanes.Count >= 0)
        {
            return;
        }

        for (int i = 0; i < dragPlanes.Count; i++)
        {
            dragPlanes[i].SetActive(true);
        }
    }

    /// <summary>
    /// Disables parent station
    /// </summary>
    public void DisableParent()
    {

        if (clickableCollider != null)
        {
            clickableCollider.enabled = false;           
        }

        if (parentLoc != null)
        {
            parentLoc.DisableParent();
        }
    }

    /// <summary>
    /// Disables this stations colliders
    /// </summary>
    public void DisableColliders()
    {
        foreach(Collider c in myClickableColliders)
        {
            c.enabled = false;
        }
    }

    /// <summary>
    /// Enables colliders for this station
    /// </summary>
    public void EnableColliders()
    {
        foreach (Collider c in myClickableColliders)
        {
            c.enabled = true;
        }

        if(clickableCollider != null)
        {
            clickableCollider.enabled = true;
        }
       
    }

    /// <summary>
    /// Returns the camera position in world coords
    /// </summary>
    /// <returns></returns>
    public Vector3 camPosWorldCoords()
    {
        return transform.TransformPoint(cameraPos);
    }

    /// <summary>
    /// Returns the rotation of the camera in world coords
    /// </summary>
    /// <returns></returns>
    public Quaternion camRotWorldCoords()
    {
        return Quaternion.Euler(transform.TransformDirection(cameraRotation.eulerAngles));
    }

}
