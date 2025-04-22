using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    private Vector3 objectOffset;

    [Header("------------ Station Variables ------------")]
    public Station parentLoc;
    public Stations stationLabel;

    public Collider clickableCollider;
    private Collider[] myClickableColliders;

    public bool invertXCameraRotation;

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
    [SerializeField] private bool separateArriveAndReturn;
    [SerializeField, ShowIf("separateArriveAndReturn")] private UnityEvent returnTo;
    [SerializeField] public bool runLeaveWhenGoingToChildren;

    public Vector3 ObjectOffset { get => transform.TransformPoint(objectOffset); set => objectOffset = value; }

#if UNITY_EDITOR
    [SerializeField]
    private bool permaGizmos = false;
    [SerializeField, Button]
    private void SetCameraPositionAndRotation() 
    {
        Camera c = SceneView.lastActiveSceneView.camera;
        cameraPos = transform.InverseTransformPoint(c.transform.position);
        cameraRotation = Quaternion.Euler(-transform.rotation.eulerAngles + transform.InverseTransformDirection(c.transform.eulerAngles));
    }
    [SerializeField, Button]
    private void ViewCamera()
    {
        SceneView v = SceneView.lastActiveSceneView;
        Camera c = v.camera;
        c.transform.position = new Vector3(0, 0, 0);
        v.pivot = transform.TransformPoint(cameraPos);
        v.rotation = Quaternion.Euler(transform.TransformVector(cameraRotation.eulerAngles));
        v.Repaint();
        //c.transform.localPosition = cameraPos;
        //c.transform.localRotation = cameraRotation;
    }
#endif

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
        if(this == StationManager.instance.playerLocation.parentLoc || Raycast.Instance.noStationMove)
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
    public virtual void OnArrive(bool forward)
    {
        if (separateArriveAndReturn && !forward)
        {
            returnTo.Invoke();
        }
        else
        {
            arrive.Invoke();
        }
        foreach(GameObject obj in stationSpecificHints)
        {
            obj.SetActive(true);
        }
    }
    
    /// <summary>
    /// Calls necessary Leave functions for this station
    /// </summary>
    public virtual void OnLeave()
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
        Gizmos.DrawWireSphere(transform.TransformPoint(objectOffset), 0.1f * transform.lossyScale.x);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(cameraPos), 0.1f * transform.lossyScale.x);
        
        Vector3 eAngle = transform.rotation.eulerAngles;
        if (invertXCameraRotation)
        {
            eAngle.x = -eAngle.x;
        }
        Matrix4x4 matrix = Matrix4x4.Translate(transform.TransformPoint(cameraPos)) * Matrix4x4.Rotate(Quaternion.Euler(eAngle + cameraRotation.eulerAngles));
        Gizmos.matrix = matrix;
        Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, Camera.main.farClipPlane * transform.lossyScale.x, Camera.main.nearClipPlane * transform.lossyScale.x, Camera.main.aspect);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * 10);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (permaGizmos)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.TransformPoint(objectOffset), 0.1f * transform.lossyScale.x);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(cameraPos), 0.1f * transform.lossyScale.x);

            Vector3 eAngle = transform.rotation.eulerAngles;
            if (invertXCameraRotation)
            {
                eAngle.x = -eAngle.x;
            }
            Matrix4x4 matrix = Matrix4x4.Translate(transform.TransformPoint(cameraPos)) * Matrix4x4.Rotate(Quaternion.Euler(eAngle + cameraRotation.eulerAngles));
            Gizmos.matrix = matrix;
            Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, Camera.main.farClipPlane * transform.lossyScale.x, Camera.main.nearClipPlane * transform.lossyScale.x, Camera.main.aspect);
        }
    }
#endif

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
        if(myClickableColliders != null)
        {
        foreach(Collider c in myClickableColliders)
            {
                if(c != null)
                    c.enabled = false;
            }
        }
    }

    /// <summary>
    /// Enables colliders for this station
    /// </summary>
    public void EnableColliders()
    {
        if(myClickableColliders.Length > 0)
        {
            foreach (Collider c in myClickableColliders)
            {
                c.enabled = true;
            }
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
        Vector3 eAngle = transform.rotation.eulerAngles;
        if (invertXCameraRotation)
        {
            eAngle.x = -eAngle.x;
        }
        return Quaternion.Euler(eAngle + cameraRotation.eulerAngles);
    }

}
