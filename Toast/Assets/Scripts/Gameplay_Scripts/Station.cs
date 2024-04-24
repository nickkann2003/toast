using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Highlights))]
public class Station : MonoBehaviour
{

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

    public void OnArrive()
    {
        arrive.Invoke();
        foreach(GameObject obj in stationSpecificHints)
        {
            obj.SetActive(true);
        }
    }

    public void OnLeave()
    {
        leave.Invoke();
        foreach (GameObject obj in stationSpecificHints)
        {
            obj.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(objectOffset, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraPos, 0.1f);
        Matrix4x4 matrix = Matrix4x4.Translate(cameraPos) * Matrix4x4.Rotate(cameraRotation);
        Gizmos.matrix = matrix;
        Gizmos.DrawFrustum(Vector3.zero, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);

    }

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

    public void DisableColliders()
    {
        foreach(Collider c in myClickableColliders)
        {
            c.enabled = false;
        }
    }

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

}
