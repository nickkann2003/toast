using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class Raycast : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Drag and Drop Prevention")]
    [ReadOnly]
    public bool noDrop = false; // also affects pickup
    [ReadOnly]
    public bool noDrag = false; // doesn't affect buttons/dials/levers
    [ReadOnly]
    public bool noStationMove = false;
    [ReadOnly]
    public bool noUse = false;

    [Header("Instances")]
    private static Raycast _instance;
    public static Raycast Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Raycast is null");
            }
            return _instance;
        }
    }

    public bool Dragging { get => dragging; }
    
    [Header("Hand Reference")]
    [SerializeField]
    public NewHand hand;

    private int layer_IgnoreRaycast = 2;
    private int layer_Interactable = 7;
    private int layer_Station = 3;
    private int layer_Plane = 10;
    private int layer_UI = 5;

    private int mask_IgnoreRaycast;
    private int mask_Interactable;
    private int mask_Station;
    private int mask_Plane;
    private int mask_UI;

    private Camera targetCamera;

    [SerializeField]
    private Camera handCam;

    private GameObject prevGO;

    [Header("Game Object References")]
    public GameObject hitGO;
    public GameObject selectGO;

    [Header("Prefabs")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject mousePointPrefab;
    [SerializeField] private int maxDistance;
    //[SerializeField] private LayerMask detectionLayer;

    [Header("Movement Variables")]
    public float scrollSpeed = 1.0f;
    private float scrollInput;
    private float stationMoveTimer = 0.0f;

    [Header("Station Movement Variables")]
    [SerializeField]
    private float stationMaxTimer = .4f;

    private GameObject line;
    private LineController lineController;
    private float mZOffset;

    private bool dragging;
    private Vector3 lastPos;
    //RaycastHit hit;
    private Highlights highlightable;
    private Highlights prevHighligtable;

    [Header("Game Event References")]
    [SerializeField]
    private PropIntGameEvent onDragEvent;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        mask_IgnoreRaycast = 1 << layer_IgnoreRaycast;
        mask_Interactable = 1 << layer_Interactable;
        mask_Station = 1 << layer_Station;
        mask_Plane = 1 << layer_Plane;
        mask_UI = 1 << layer_UI;
        prevGO = null;
        hitGO = null;
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stationMoveTimer > 0.0f)
        {
            stationMoveTimer -= Time.deltaTime;
        }

        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        prevGO = hitGO;
        if (Input.GetButtonUp("Drag"))
        {
            if (dragging)
            {
                StopDragging();
            }
            
        }
        if (!dragging)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // turn off prev highlight
                if (prevHighligtable != null)
                {
                    prevHighligtable.TurnOffHighlight();
                }
                hitGO = null;
                return;
            }
        }
        TestRaycast();
        if (Input.GetButtonDown("Drag") && !noDrag)
        {
            //print($"Object: \"{hitGO.name}\"");

            StartDragging();
        }
        if (Input.GetButtonDown("View"))
        {
            ViewRaycast();
        }

        // OBJECT EAT DETECTION
        if (Input.GetButtonDown("Use") && !noUse)
        {
            if (hand.CheckObject())
            {
                hand.TryUseInHand();
                return;
            }
            UseRaycast();
        }

        // OBJECT PICKUP DETECTION
        if (Input.GetButtonDown("Pickup") && !noDrop)
        {
            PickupRaycast(hand);
        }


        if (dragging)
        {
            if (!Input.GetButton("Drag") || noDrag)
            {
                StopDragging();
            }
            if (selectGO != null && selectGO.name != "SM_CounterDrawer") // HARDCODE CHANGE LATER
            {

                if (selectGO.GetComponent<Rigidbody>() != null /*&& StationManager.instance.playerLocation.dragPlane != null*/)
                {
                    // Plane-based code
                    // Create ray and hit
                    RaycastHit hit;
                    Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

                    // Check for hit
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, (mask_Plane & ~mask_IgnoreRaycast))
                            /*&& hit.collider.gameObject == StationManager.instance.playerLocation.dragPlane*/)
                    {
                        // Set velocity based on position on plane
                        selectGO.GetComponent<Rigidbody>().velocity =
                        (new Vector3(hit.point.x, hit.point.y,
                        hit.point.z) - selectGO.transform.position) * 10;

                        //Debug.Log($"Point on Plane: ({hit.point.x}, {hit.point.y}, {hit.point.z}");

                        lastPos = new Vector3(hit.point.x, hit.point.y,
                        hit.point.z);
                    }
                }
            }
            else
            {
                StopDragging();
            }
        }
    }

    /// <summary>
    /// Triggers the examine manager on the hovered item
    /// </summary>
    void ViewRaycast()
    {
        if (dragging && selectGO.GetComponent<NewProp>() != null)
        {
            ExamineManager.instance.ExamineObject(selectGO);
            PieManager.instance.ExamineObject.RaiseEvent(selectGO.GetComponent<NewProp>(), 1);
            return;
        }
        
        RaycastHit hit = RaycastHelper(~mask_Station);

        GameObject itemToView = null;
        if (hit.collider != null)
        {
            itemToView = hit.collider.gameObject;
        }

        if (itemToView != null && itemToView.GetComponent<NewProp>() != null)
        {
            ExamineManager.instance.ExamineObject(hitGO);
            PieManager.instance.ExamineObject.RaiseEvent(itemToView.GetComponent<NewProp>(), 1);
            StopDragging();
            return;
        }

        if (hand.CheckObject())
        {
            ExamineManager.instance.ExamineObject(hand.CheckObject());
            PieManager.instance.ExamineObject.RaiseEvent(hand.GetComponent<NewProp>(), 1);
            StopDragging();
            return;
        }
    }

    /// <summary>
    /// Sends a ray and gets current item
    /// </summary>
    void UseRaycast()
    {
        RaycastHit hit = RaycastHelper(~mask_Station);

        GameObject itemToUse = null;
        if (hit.collider != null)
        {
            itemToUse = hit.collider.gameObject;
            //Debug.Log(itemToUse);
        }
        

        if (itemToUse != null && itemToUse.GetComponent<NewProp>() != null && itemToUse.layer == layer_Interactable) // Interactable layer
        {
            itemToUse.GetComponent<NewProp>().Use();
            //// !!MOVE THIS TO THE EATABLE COMPONENT!! //
            //Color c = itemToUse.GetComponent<Renderer>().material.color;
            //var main = Camera.main.GetComponent<Hand>().EatParticles.main;
            //main.startColor = c;
            //RequirementEvent rEvent;
            //if (itemToUse.GetComponent<IEatable>().BitesLeft() <= 1)
            //{
            //    if (itemToUse.GetComponent<ObjectVariables>() != null)
            //    {
            //        rEvent = new RequirementEvent(RequirementType.EatObject, itemToUse.GetComponent<ObjectVariables>(), true);
            //    }
            //    else
            //    {
            //        rEvent = new RequirementEvent(RequirementType.EatObject, new ObjectVariables(), true);
            //    }
            //    ObjectiveManager.instance.UpdateObjectives(rEvent);
            //}
            //// ^^MOVE THIS TO THE EATABLE COMPONENT^^ //

            //itemToUse.GetComponent<IEatable>().TakeBite();
            //Camera.main.GetComponent<Hand>().EatParticles.Play();

            //prevGO = null;
            //hitGO = null;
            //prevHighligtable = null;
            //highlightable = null;
        }
    }

    /// <summary>
    /// Return true if item was picked up, false otherwise
    /// </summary>
    /// <param name="_hand">Hand to pickup item</param>
    /// <returns></returns>
    public bool PickupRaycast(NewHand _hand)
    {
        if (_hand==null)
        {
            Debug.Log("Hand is null");
            return false;
        }

        GameObject itemToDrop = _hand.Drop();
        if (itemToDrop != null)
        {
            Station currentStation = StationManager.instance.playerLocation;

            // Grab world pos from the current station
            Vector3 objectPos = currentStation.ObjectOffset;
            Vector3 objectDropRotation = new Vector3(0, 90, 0);
            objectDropRotation += currentStation.gameObject.transform.rotation.eulerAngles;

            RaycastHit hitTest;
            Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

            // Check for hit
            if (Physics.Raycast(ray, out hitTest, Mathf.Infinity, (~mask_IgnoreRaycast & ~mask_Station)))
            {
                Vector3 offset = new Vector3(0, .3f * hitTest.normal.y, 0);
                if (offset.y == 0)
                {
                    offset.y = .3f;
                }

                objectPos = hitTest.point + offset;
            }

            // Set the current position of the object
            itemToDrop.transform.position = objectPos;
            itemToDrop.transform.rotation = Quaternion.identity;
            itemToDrop.transform.Rotate(objectDropRotation);

            // Try setting rigidbody, catch if object doesnt have a rigidbody
            //try
            //{
            //    itemToDrop.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //}
            //catch
            //{
            //    Debug.Log("Dropped item without a rigid body");
            //}


            return true;
        }

        if (dragging && selectGO.GetComponent<NewProp>() != null)
        {
            if (selectGO.GetComponent<NewProp>().HasFlag(PropFlags.ImmuneToPickup))
            {
                return false;
            }
            hand.Pickup(selectGO);
            StopDragging();
            return true;
        }

        // shoot out a raycast, ignore every layer except interactable
        RaycastHit hit = RaycastHelper(~mask_Station);

        GameObject itemToPickup = null;
        if (hit.collider != null)
        {
            itemToPickup = hit.collider.gameObject;
        }

        if (itemToPickup == null)
        {
            return false;
        }

        if (itemToPickup.GetComponent<NewProp>() != null) // Interactable layer
        {
            if (itemToPickup.GetComponent<NewProp>().HasFlag(PropFlags.ImmuneToPickup))
            {
                return false;
            }
            
            // Carrier check
            Carrier c = null;
            itemToPickup.TryGetComponent<Carrier>(out c);
            if (c != null)
            {
                c.PickUp();
            }

            hit.collider.gameObject.transform.position = new Vector3(100, 100, 100);
            StartCoroutine(PickupItem(itemToPickup, _hand));
            return true;
        }
        return false;
    }

    private IEnumerator PickupItem(GameObject itemToPickup, NewHand _hand)
    {
        yield return new WaitForFixedUpdate();
        _hand.Pickup(itemToPickup);
    }

    /// <summary>
    /// Perform a raycast hit, or check to move stations based on scroll input
    /// </summary>
    void TestRaycast()
    {
        if (scrollInput < 0f && stationMoveTimer <= 0 && !noStationMove)
        {
            StationManager.instance.StationMoveBack();
            stationMoveTimer = stationMaxTimer;
        }
        if (!dragging)
        {
            // turn off prev highlight
            if (prevHighligtable != null)
            {
                prevHighligtable.TurnOffHighlight();
            }
            // swap highlight over
            prevHighligtable = highlightable;
        }

        //hit = new RaycastHit();
        //Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = RaycastHelper(~mask_IgnoreRaycast);

        hitGO = null;
        if (hit.collider != null)
        {
            hitGO = hit.collider.gameObject;
        }

        if (hitGO != null && (hitGO.layer == layer_Station || hitGO.layer == layer_Interactable))
        {
            //hitGO = hit.collider.gameObject;

            highlightable = hitGO.GetComponent<Highlights>();
            if (highlightable != null)
            {
                GameManager.Instance.SetHandCursor();
                if (!dragging)
                {
                    highlightable.TurnOnHighlght();
                }
                Station hightLocation = hitGO.GetComponent<Station>();
                if (hightLocation)
                {
                    if ((scrollInput > 0f || Input.GetButtonDown("Drag")) && stationMoveTimer <= 0)
                    {
                        //Debug.Log("Try to zoom in" + hightLocation);
                        StationManager.instance.MoveToStation(hightLocation);
                        stationMoveTimer = stationMaxTimer;
                    }
                }
            }
            
        }
        else
        {
            if (!dragging)
            {
                GameManager.Instance.SetDefaultCursor();
                if (highlightable != null)
                {

                    highlightable.TurnOffHighlight();
                    prevHighligtable = highlightable;
                    highlightable = null;
                }
            }
        }

    }

    /// <summary>
    /// Take a raycast hit and start dragging
    /// </summary>
    void StartDragging()
    {
        if (hitGO == null)
        {
            return;
        }

        if (hitGO.GetComponent<NewProp>() != null)
        {
            if (hitGO.GetComponent<NewProp>().HasFlag(PropFlags.ImmuneToDrag))
            {
                return;
            }
            if (hitGO.GetComponent<NewProp>().HasFlag(PropFlags.InHand))
            {
                Debug.Log("Forcibly removing from hand");
                hitGO.GetComponent<NewProp>().ForceRemoveFromHand();

                // Handle dropping item in hand when dragged
                Station currentStation = StationManager.instance.playerLocation;
                GameObject itemToDrop = hitGO;
                Vector3 objectPos = currentStation.ObjectOffset;
                Vector3 objectDropRotation = new Vector3(0, 90, 0);
                objectDropRotation += currentStation.gameObject.transform.rotation.eulerAngles;
                RaycastHit hitTest;
                Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

                // Check for hit
                if (Physics.Raycast(ray, out hitTest, Mathf.Infinity, (~mask_IgnoreRaycast & ~mask_Station)))
                {
                    Vector3 offset = new Vector3(0, .3f * hitTest.normal.y, 0);
                    if (offset.y == 0)
                    {
                        offset.y = .3f;
                    }

                    objectPos = hitTest.point + offset;
                }

                // Set the current position of the object
                itemToDrop.transform.position = objectPos;
                itemToDrop.transform.rotation = Quaternion.identity;
                itemToDrop.transform.Rotate(objectDropRotation);

                // Spread specific, mark as not on knife
                if (hitGO.TryGetComponent(out Spread spread))
                {
                    spread.IsOnKnife = false;
                }
            }
            onDragEvent.RaiseEvent(hitGO.GetComponent<NewProp>(), 1);
        }

        if (hitGO.GetComponent<Rigidbody>() != null || hitGO.name == "Toaster_Lever" || hitGO.name == "Toaster_Dial") // HARDCODE FOR NOW CHANGE LATER
        {
            line = Instantiate(linePrefab);
            lineController = line.GetComponent<LineController>();

            selectGO = hitGO;
            mZOffset = Camera.main.WorldToScreenPoint(selectGO.transform.position).z;
            line.GetComponent<LineController>().SetAnchor(selectGO.transform);

            //// HARDCODED
            //if (hitGO.name == "Toaster_Lever")
            //{
            //    // give the line for the lever a slight offset to make it visible
            //    Vector3 offset = Vector3.zero;
            //    offset.z = hit.point.z - selectGO.transform.position.z;
            //    lineController.SetOffset(offset);
            //}
        }
        dragging = true;
    }

    /// <summary>
    /// Perform stop dragging operations
    /// </summary>
    public void StopDragging()
    {
        if (selectGO != null)
        {
            selectGO = null;
        }

        Destroy(line);
        line = null;

        dragging = false;
    }   

    /// <summary>
    /// Helper function that returns a raycast hit based on mouse position
    /// </summary>
    /// <param name="layerMask"></param>
    /// <returns></returns>
    public RaycastHit RaycastHelper(int layerMask, bool ignoreHandCam = false)
    {
        RaycastHit hit = new RaycastHit();

        Ray ray = handCam.ScreenPointToRay(Input.mousePosition);
        if (!ignoreHandCam)
        {
            // shoot a raycast, ignoring the layermask
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, (layerMask & ~mask_IgnoreRaycast & ~mask_Plane & ~mask_UI))
                && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.distance <= 5)
                {
                    return hit;
                }
            }
        }

        ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        // shoot a raycast, ignoring the layermask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (layerMask & ~mask_IgnoreRaycast & ~mask_Plane & ~mask_UI))
            && !EventSystem.current.IsPointerOverGameObject())
        {
            return hit;
        }

        // draw ray at hit point
        Debug.DrawLine(ray.origin, hit.point, Color.yellow);
        //Debug.DrawRay(hit.point, Vector3.Reflect(transform.InverseTransformPoint(hit.point), hit.normal), Color.blue);

        return hit;
    }

    //Vector3 OffPlaneHelper(Vector3 planePos)
    //{
    //    Vector3 newVelocity = planePos;

    //    Vector3 planeMin = StationManager.instance.playerLocation.dragPlane.GetComponent<MeshRenderer>().bounds.min;
    //    Vector3 planeMax = StationManager.instance.playerLocation.dragPlane.GetComponent<MeshRenderer>().bounds.max;

    //    Vector3 mouseCopy = Input.mousePosition;
    //    mouseCopy.z = targetCamera.WorldToScreenPoint(planePos).z;
    //    mouseCopy = targetCamera.ScreenToWorldPoint(mouseCopy);

    //    //Debug.Log("Mouse pos:" + Input.mousePosition);
    //    //Debug.Log("Plane min x: " + targetCamera.WorldToScreenPoint(planeMin).x);

    //    if (Input.mousePosition.x < targetCamera.WorldToScreenPoint(planeMin).x)
    //    {
    //        newVelocity.x = planeMin.x;
    //    }
    //    else if(Input.mousePosition.x > targetCamera.WorldToScreenPoint(planeMax).x)
    //    {
    //        newVelocity.x = planeMax.x;
    //    }
    //    else
    //    {
    //        newVelocity.x = mouseCopy.x;
    //    }

    //    if (Input.mousePosition.y < targetCamera.WorldToScreenPoint(planeMin).y)
    //    {
    //        newVelocity.y = planeMin.y;
    //    }
    //    else if (Input.mousePosition.y > targetCamera.WorldToScreenPoint(planeMax).y)
    //    {
    //        newVelocity.y = planeMax.y;
    //    }
    //    else
    //    {
    //        newVelocity.y = mouseCopy.y;
    //    }

    //    if ((targetCamera.transform.rotation * Input.mousePosition).z < targetCamera.WorldToScreenPoint(planeMin).x)
    //    {
    //        newVelocity.z = planeMin.z;
    //    }
    //    else if ((targetCamera.transform.rotation * Input.mousePosition).z > targetCamera.WorldToScreenPoint(planeMax).x)
    //    {
    //        newVelocity.z = planeMax.z;
    //    }
    //    else
    //    {
    //        newVelocity.z = mouseCopy.z;
    //    }

    //    return newVelocity;
    //}

    /// <summary>
    /// Checks if knives are stacked
    /// </summary>
    /// <returns>The number of knives stacked on top of each other</returns>
    public int CheckKnifeStack()
    {
        NewHand handToCheck = hand;

        int knives = 0;

        while (handToCheck != null)
        {
            GameObject objInHand = handToCheck.CheckObject();
            if (objInHand != null && objInHand.GetComponent<Knife>() != null)
            {
                handToCheck = objInHand.GetComponent<Knife>().hand;
                knives++;
            }
            else
            {
                handToCheck = null;
            }
        }

        return knives;
    }
}
