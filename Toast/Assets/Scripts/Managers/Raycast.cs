using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    private int layer_IgnoreRaycast = 2;
    private int layer_Interactable = 7;
    private int layer_Station = 3;

    private int mask_IgnoreRaycast;
    private int mask_Interactable;
    private int mask_Station;

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
    private float stationMaxTimer = .1f;

    GameObject line;
    LineController lineController;
    float mZOffset;

    bool dragging;
    //RaycastHit hit;

    private Highlights highlightable;
    private Highlights prevHighligtable;

    // Start is called before the first frame update
    void Awake()
    {
        mask_IgnoreRaycast = 1 << layer_IgnoreRaycast;
        mask_Interactable = 1 << layer_Interactable;
        mask_Station = 1 << layer_Station;
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
        if (Input.GetButtonDown("Drag"))
        {
            //print($"Object: \"{hitGO.name}\"");

            StartDragging();
        }
        if (Input.GetButtonUp("Drag"))
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
            UseRaycast();
        }

        // OBJECT PICKUP DETECTION
        if (Input.GetButtonDown("Pickup"))
        {
            PickupRaycast();
        }


        if (dragging)
        {
            if (!Input.GetButton("Drag"))
            {
                StopDragging();
            }
            if (selectGO != null && selectGO.name != "SM_CounterDrawer") // HARDCODE CHANGE LATER
            {
                if (selectGO.GetComponent<Rigidbody>() != null)
                {
                    selectGO.GetComponent<Rigidbody>().velocity =
                    (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    mZOffset)) - selectGO.transform.position) * 10;
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
        if (dragging && selectGO.GetComponent<Prop>() != null)
        {
            ExamineManager.instance.ExamineObject(selectGO.GetComponent<Prop>());
            return;
        }

        GameObject itemToView = RaycastHelper(~mask_Station);

        if (itemToView != null && itemToView.GetComponent<Prop>() != null)
        {
            ExamineManager.instance.ExamineObject(hitGO.GetComponent<Prop>());
        }
        StopDragging();
    }

    void UseRaycast()
    {
        GameObject itemToUse = RaycastHelper(~mask_Station);

        if (itemToUse != null && itemToUse.GetComponent<IEatable>() != null && itemToUse.layer == layer_Interactable) // Interactable layer
        {
            // !!MOVE THIS TO THE EATABLE COMPONENT!! //
            Color c = itemToUse.GetComponent<Renderer>().material.color;
            var main = Camera.main.GetComponent<Hand>().EatParticles.main;
            main.startColor = c;
            RequirementEvent rEvent;
            if (itemToUse.GetComponent<IEatable>().BitesLeft() <= 1)
            {
                if (itemToUse.GetComponent<ObjectVariables>() != null)
                {
                    rEvent = new RequirementEvent(RequirementType.EatObject, itemToUse.GetComponent<ObjectVariables>(), true);
                }
                else
                {
                    rEvent = new RequirementEvent(RequirementType.EatObject, new ObjectVariables(), true);
                }
                ObjectiveManager.instance.UpdateObjectives(rEvent);
            }
            // ^^MOVE THIS TO THE EATABLE COMPONENT^^ //

            itemToUse.GetComponent<IEatable>().TakeBite();
            Camera.main.GetComponent<Hand>().EatParticles.Play();

            //prevGO = null;
            //hitGO = null;
            //prevHighligtable = null;
            //highlightable = null;
        }
    }

    void PickupRaycast()
    {
        if (dragging && selectGO.GetComponent<Prop>() != null)
        {
            Camera.main.GetComponent<Hand>().AddItem(selectGO);
            StopDragging();
            return;
        }

        // shoot out a raycast, ignore every layer except interactable
        GameObject itemToPickup = RaycastHelper(~mask_Station);

        if (itemToPickup != null && itemToPickup.GetComponent<Prop>() != null) // Interactable layer
        {
            Camera.main.GetComponent<Hand>().AddItem(itemToPickup);
            //prevGO = null;
            //hitGO = null;
            //prevHighligtable = null;
            //highlightable = null;
        }
    }

    void TestRaycast()
    {

        if (scrollInput < 0f)
        {
            StationManager.instance.StationMoveBack();
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (layerMask & ~mask_IgnoreRaycast)))
        {
            return hit.collider.gameObject;
        }

        // draw ray at hit point
        Debug.DrawLine(ray.origin, hit.point, Color.yellow);
        //Debug.DrawRay(hit.point, Vector3.Reflect(transform.InverseTransformPoint(hit.point), hit.normal), Color.blue);

        return null;
    }
}
