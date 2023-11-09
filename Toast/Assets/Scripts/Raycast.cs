using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    Camera targetCamera;

    GameObject prevGO;
    public GameObject hitGO;

    public GameObject selectGO;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject mousePointPrefab;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask detectionLayer;

    GameObject line;
    LineController lineController;
    float mZOffset;

    bool dragging;
    RaycastHit hit;

    private IHighlightable highlightable;
    private IHighlightable prevHighligtable;

    // Start is called before the first frame update
    void Awake()
    {
        prevGO = null;
        hitGO = null;
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        prevGO = hitGO;
        TestRaycast();
        if (Input.GetButtonDown("Drag"))
        {
            print($"Object: \"{hitGO.name}\"");

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
            if (dragging && selectGO.GetComponent<Prop>() != null)
            {
                ExamineManager.instance.ExamineObject(selectGO.GetComponent<Prop>());
            }
            else if (hitGO.GetComponent<Prop>())
            {
                ExamineManager.instance.ExamineObject(hitGO.GetComponent<Prop>());
            }
            StopDragging();
        }

        // OBJECT EAT DETECTION
        if (Input.GetButtonDown("Use"))
        {
            if (hitGO != null && hitGO.layer == 7) // Interactable layer
            {
                if (hitGO.GetComponent<IEatable>() != null)
                {
                    Color c = hitGO.GetComponent<Renderer>().material.color;
                    var main = Camera.main.GetComponent<Hand>().EatParticles.main;
                    main.startColor = c;
                    RequirementEvent rEvent;
                    if (hitGO.GetComponent<IEatable>().BitesLeft() <= 1)
                    {
                        if (hitGO.GetComponent<ObjectVariables>() != null)
                        {
                            rEvent = new RequirementEvent(RequirementType.EatObject, hitGO.GetComponent<ObjectVariables>(), true);
                        }
                        else
                        {
                            rEvent = new RequirementEvent(RequirementType.EatObject, new ObjectVariables(), true);
                        }
                        ObjectiveManager.instance.UpdateObjectives(rEvent);
                    }
                    hitGO.GetComponent<IEatable>().TakeBite();
                    Camera.main.GetComponent<Hand>().EatParticles.Play();
                    prevGO = null;
                    hitGO = null;
                    prevHighligtable = null;
                    highlightable = null;
                }
            }
        }

        // OBJECT PICKUP DETECTION
        if (Input.GetButtonDown("Pickup"))
        {
            if (hitGO != null && hitGO.layer == 7) // Interactable layer
            {
                Camera.main.GetComponent<Hand>().AddItem(hitGO);
                prevGO = null;
                hitGO = null;
                prevHighligtable = null;
                highlightable = null;
            }
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

    void TestRaycast()
    {
        // turn off prev highlight
        if (prevHighligtable != null)
        {
            prevHighligtable.TurnOffHighlight();
        }
        // swap highlight over
        prevHighligtable = highlightable;


        hit = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            hitGO = hit.collider.gameObject;

            if (hitGO.layer == 7) // interactable layer
            {
                highlightable = hit.collider.GetComponent<IHighlightable>();
                if (highlightable != null)
                {
                    GameManager.Instance.SetHandCursor();
                    highlightable.TurnOnHighlght();
                }
            }
            else
            {
                GameManager.Instance.SetDefaultCursor();
            }


            if (hitGO != prevGO)
            {
                //print($"Object: \"{hitGO.name}\"");
            }
            
            

            // draw ray at hit point
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            //Debug.DrawRay(hit.point, Vector3.Reflect(transform.InverseTransformPoint(hit.point), hit.normal), Color.blue);
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
}
