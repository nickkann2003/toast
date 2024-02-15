using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Highlights))]
public class Station : MonoBehaviour
{
    [SerializeField] private UnityEvent arrive;
    [SerializeField] private UnityEvent leave;

    public Vector3 cameraPos;
    public Quaternion cameraRotation = Quaternion.identity;
    public Stations stationLabel;
    public Vector3 objectOffset;
    public Station parentLoc;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    public Collider clickableCollider;
    //public GameObject stationHighlight;

    private Highlights highlight;

    // If isEnable is false, then the player should not be able to reach to interact with this station/prop
    [SerializeField] private bool isEnabled;


    public GameObject dragPlane;

    ///private int layer_Station = 3;
    ///private int layer_UI = 5;
    ///private int layer_Ignore = 2;
    ///private int mask_Station;
    ///private int mask_UI;
    ///private int mask_Ignore;

    //private void Awake()
    //{
    //    mask_Station = 1 << layer_Station;
    //    mask_UI = 1 << layer_UI;
    //    mask_Ignore = 1 << layer_Ignore;
    //}

    // Start is called before the first frame update
    void Start()
    {
        isEnabled = true;
        if (clickableCollider != null)
        {
            clickableCollider = GetComponent<Collider>();
        }
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~mask_Ignore))
    //        {
    //            if (hit.collider == clickableCollider)
    //            {
                   
    //            }
    //        }
    //    }
    //}

    private void OnMouseDown()
    {
        if (StationManager.instance != null && StationManager.instance.playerLocation != null)
        {
            StationManager.instance.MoveToStation(this);
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
    }

    public void OnLeave()
    {
        leave.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraPos, 0.1f);
        Gizmos.DrawFrustum(cameraPos, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(objectOffset, 0.1f);
    }

}
