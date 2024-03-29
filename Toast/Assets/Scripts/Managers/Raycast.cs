using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class Raycast : MonoBehaviour
{
    public bool noDrop = false;
    public bool noDrag = false;

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

    [SerializeField]
    public NewHand hand;

    private int layer_IgnoreRaycast = 2;
    private int layer_Interactable = 7;
    private int layer_Station = 3;
    private int layer_Plane = 10;

    private int mask_IgnoreRaycast;
    private int mask_Interactable;
    private int mask_Station;
    private int mask_Plane;

    Camera targetCamera;

    GameObject prevGO;
    public GameObject hitGO;

    public GameObject selectGO;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject mousePointPrefab;
    [SerializeField] private int maxDistance;
    //[SerializeField] private LayerMask detectionLayer;

    public float scrollSpeed = 1.0f;
    private float scrollInput;
    private float stationMoveTimer = 0.0f;
    private float stationMaxTimer = .2f;

    GameObject line;
    LineController lineController;
    float mZOffset;

    bool dragging;
    private Vector3 lastPos;
    //RaycastHit hit;

    private Highlights highlightable;
    private Highlights prevHighligtable;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;

        mask_IgnoreRaycast = 1 << layer_IgnoreRaycast;
        mask_Interactable = 1 << layer_Interactable;
        mask_Station = 1 << layer_Station;
        mask_Plane = 1 << layer_Plane;
        prevGO = null;
        hitGO = null;
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stationMoveTimer > 0.0f)
        {
            stationMoveTimer -= Time.deltaTime;
        }

        scrollInput = Input.GetAxis("Mouse ScrollWheel");
        prevGO = hitGO;
        TestRaycast();
        if (Input.GetButtonDown("Drag") && !noDrag)
        {
            //print($"Object: \"{hitGO.name}\"");

            StartDragging();
        }
        if (Input.GetButtonUp("Drag") && !noDrag)
        {
            if (dragging)
            {
                StopDragging();
            }
            
        }
        if (Input.GetButtonDown("View"))
        {
            ViewRaycast();
        }

        // OBJECT EAT DETECTION
        if (Input.GetButtonDown("Use"))
        {
            if (hand.CheckObject())
            {
                hand.UseInHand();
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
            if (!Input.GetButton("Drag") && !noDrag)
            {
                StopDragging();
            }
            if (selectGO != null && selectGO.name != "SM_CounterDrawer") // HARDCODE CHANGE LATER
            {

                if (selectGO.GetComponent<Rigidbody>() != null && StationManager.instance.playerLocation.dragPlane != null)
                {
                    // Plane-based code
                    // Create ray and hit
                    RaycastHit hit;
                    Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

                    // Check for hit
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, (mask_Plane & ~mask_IgnoreRaycast))
                            && hit.collider.gameObject == StationManager.instance.playerLocation.dragPlane)
                    {
                        // Set velocity based on position on plane
                        selectGO.GetComponent<Rigidbody>().velocity =
                        (new Vector3(hit.point.x, hit.point.y,
                        hit.point.z) - selectGO.transform.position) * 10;

                        //Debug.Log($"Point on Plane: ({hit.point.x}, {hit.point.y}, {hit.point.z}");

                        lastPos = new Vector3(hit.point.x, hit.point.y,
                        hit.point.z);
                    }
                    else
                    {
                        Debug.Log("Dragging failed");

                        // Set velocity based on last stored position on plane
                        selectGO.GetComponent<Rigidbody>().velocity =
                        (OffPlaneHelper(lastPos) - selectGO.transform.position) * 10;
                        
                        
                    }
                }
            }
            else
            {
                StopDragging();
            }
        }
    }

    void ViewRaycast()
    {
        if (dragging && selectGO.GetComponent<NewProp>() != null)
        {
            ExamineManager.instance.ExamineObject(selectGO);
            return;
        }

        GameObject itemToView = RaycastHelper(~mask_Station);

        if (itemToView != null && itemToView.GetComponent<NewProp>() != null)
        {
            ExamineManager.instance.ExamineObject(hitGO);
            StopDragging();
            return;
        }

        if (hand.CheckObject())
        {
            ExamineManager.instance.ExamineObject(hand.CheckObject());
            StopDragging();
            return;
        }
    }

    void UseRaycast()
    {
        GameObject itemToUse = RaycastHelper(~mask_Station);

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

    public void PickupRaycast(NewHand _hand)
    {
        if (_hand==null)
        {
            Debug.Log("Hand is null");
            return;
        }

        GameObject itemToDrop = _hand.Drop();
        if (itemToDrop != null)
        {
            Station currentStation = StationManager.instance.playerLocation;

            // Grab world pos from the current station
            Vector3 objectPos = currentStation.objectOffset;

            // Set the current position of the object
            itemToDrop.transform.position = objectPos;

            // Try setting rigidbody, catch if object doesnt have a rigidbody
            try
            {
                itemToDrop.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            catch
            {
                Debug.Log("Dropped item without a rigid body");
            }


            return;
        }

        if (dragging && selectGO.GetComponent<NewProp>() != null)
        {
            if (selectGO.GetComponent<NewProp>().HasAttribute(PropFlags.ImmuneToPickup))
            {
                return;
            }
            hand.Pickup(selectGO);
            StopDragging();
            return;
        }

        // shoot out a raycast, ignore every layer except interactable
        GameObject itemToPickup = RaycastHelper(~mask_Station);

        if (itemToPickup == null)
        {
            return;
        }

        if (itemToPickup.GetComponent<NewProp>() != null) // Interactable layer
        {
            if (itemToPickup.GetComponent<NewProp>().HasAttribute(PropFlags.ImmuneToPickup))
            {
                return;
            }
            _hand.Pickup(itemToPickup);
        }
    }

    void TestRaycast()
    {

        if (scrollInput < 0f && stationMoveTimer <= 0)
        {
            StationManager.instance.StationMoveBack();
            stationMoveTimer = stationMaxTimer;
        }
        // turn off prev highlight
        if (prevHighligtable != null)
        {
            prevHighligtable.TurnOffHighlight();
        }
        // swap highlight over
        prevHighligtable = highlightable;


        //hit = new RaycastHit();
        //Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        hitGO = RaycastHelper(~mask_IgnoreRaycast);

        if (hitGO != null && (hitGO.layer == layer_Station || hitGO.layer == layer_Interactable))
        {
            //hitGO = hit.collider.gameObject;

            highlightable = hitGO.GetComponent<Highlights>();
            if (highlightable != null)
            {
                    GameManager.Instance.SetHandCursor();
                    highlightable.TurnOnHighlght();

                Station hightLocation = hitGO.GetComponent<Station>();
                if (hightLocation)
                {
                    if ((scrollInput > 0f || Input.GetButtonDown("Drag")) && stationMoveTimer <= 0)
                    {
                        Debug.Log("Try to zoom in" + hightLocation);
                        StationManager.instance.MoveToStation(hightLocation);
                        stationMoveTimer = stationMaxTimer;
                    }
                }
            }
            
        }
        else
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

    void StartDragging()
    {
        if (hitGO == null)
        {
            return;
        }

        if (hitGO.GetComponent<NewProp>() != null)
        {
            if (hitGO.GetComponent<NewProp>().HasAttribute(PropFlags.ImmuneToDrag))
            {
                return;
            }
            if (hitGO.GetComponent<NewProp>().HasAttribute(PropFlags.InHand))
            {
                Debug.Log("Forcibly removing from hand");
                hitGO.GetComponent<NewProp>().ForceRemoveFromHand();
            }
            ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.DragObject, hitGO.GetComponent<NewProp>().attributes, true));
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

    void StopDragging()
    {
        if (selectGO != null)
        {
            selectGO = null;
        }

        Destroy(line);
        line = null;

        dragging = false;
    }

    GameObject RaycastHelper(int layerMask)
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);

        // shoot a raycast, ignoring the layermask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (layerMask & ~mask_IgnoreRaycast & ~mask_Plane)))
        {
            return hit.collider.gameObject;
        }

        // draw ray at hit point
        Debug.DrawLine(ray.origin, hit.point, Color.yellow);
        //Debug.DrawRay(hit.point, Vector3.Reflect(transform.InverseTransformPoint(hit.point), hit.normal), Color.blue);

        return null;
    }

    Vector3 OffPlaneHelper(Vector3 planePos)
    {
        Vector3 newVelocity = planePos;

        Vector3 planeMin = StationManager.instance.playerLocation.dragPlane.GetComponent<MeshRenderer>().bounds.min;
        Vector3 planeMax = StationManager.instance.playerLocation.dragPlane.GetComponent<MeshRenderer>().bounds.max;

        Vector3 mouseCopy = Input.mousePosition;
        mouseCopy.z = targetCamera.WorldToScreenPoint(planePos).z;
        mouseCopy = targetCamera.ScreenToWorldPoint(mouseCopy);

        //Debug.Log("Mouse pos:" + Input.mousePosition);
        //Debug.Log("Plane min x: " + targetCamera.WorldToScreenPoint(planeMin).x);

        if (Input.mousePosition.x < targetCamera.WorldToScreenPoint(planeMin).x)
        {
            newVelocity.x = planeMin.x;
        }
        else if(Input.mousePosition.x > targetCamera.WorldToScreenPoint(planeMax).x)
        {
            newVelocity.x = planeMax.x;
        }
        else
        {
            newVelocity.x = mouseCopy.x;
        }

        if (Input.mousePosition.y < targetCamera.WorldToScreenPoint(planeMin).y)
        {
            newVelocity.y = planeMin.y;
        }
        else if (Input.mousePosition.y > targetCamera.WorldToScreenPoint(planeMax).y)
        {
            newVelocity.y = planeMax.y;
        }
        else
        {
            newVelocity.y = mouseCopy.y;
        }

        if ((targetCamera.transform.rotation * Input.mousePosition).z < targetCamera.WorldToScreenPoint(planeMin).x)
        {
            newVelocity.z = planeMin.z;
        }
        else if ((targetCamera.transform.rotation * Input.mousePosition).z > targetCamera.WorldToScreenPoint(planeMax).x)
        {
            newVelocity.z = planeMax.z;
        }
        else
        {
            newVelocity.z = mouseCopy.z;
        }

        return newVelocity;
    }

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
