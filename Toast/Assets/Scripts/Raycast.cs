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
        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                StopDragging();
            }
            
        }

        if (dragging)
        {
            if (selectGO != null && selectGO.name != "SM_CounterDrawer")
            {
                if (selectGO.GetComponent<Rigidbody>() != null)
                {
                    selectGO.GetComponent<Rigidbody>().velocity =
                    (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    mZOffset)) - selectGO.transform.position) * 10;
                    if (Input.GetKeyDown("e"))
                    {
                        Camera.main.GetComponent<Hand>().AddItem(selectGO);
                    }
                }
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
                print($"Object: \"{hitGO.name}\"");
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

        if (line != null)
        {
            Destroy(line);
            line = null;
        }

        dragging = false;
    }
}
