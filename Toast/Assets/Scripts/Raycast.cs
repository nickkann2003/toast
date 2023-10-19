using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    Camera targetCamera;

    GameObject prevGO;
    GameObject hitGO;

    GameObject selectGO;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject mousePointPrefab;
    [SerializeField] private int maxDistance;
    [SerializeField] private LayerMask detectionLayer;

    GameObject mousePoint;
    GameObject springJoint;
    GameObject line;
    float mZOffset;

    bool dragging;

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
    }

    void TestRaycast()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, maxDistance,detectionLayer))
        {
            
            hitGO = hit.collider.gameObject;
            highlightable = hit.collider.GetComponent<IHighlightable>();
            if (highlightable != null)
            {
                GameManager.Instance.SetHandCursor();
                highlightable.TurnOnHighlght();
                if (prevHighligtable != highlightable && prevHighligtable != null)
                {
                    prevHighligtable.TurnOffHighlight();
                    prevHighligtable = highlightable;
                }

            }


            if (hitGO != prevGO)
            {
                print($"Object: \"{hitGO.name}\"");
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                print($"Object: \"{hitGO.name}\"");
                selectGO = hitGO;

                line = Instantiate(linePrefab);         

                if (selectGO.GetComponent<Rigidbody>() != null)
                {
                    //StartDragging(hit.point);
                    mZOffset = Camera.main.WorldToScreenPoint(selectGO.transform.position).z;
                    line.GetComponent<LineController>().SetAnchor(selectGO.transform);
                }
                else
                {
                    line.GetComponent<LineController>().SetAnchor(selectGO.transform);
                }
                dragging = true;


                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = hit.point;
                //hitGO.transform.parent = cube.transform;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (mousePoint != null)
                {
                    //StopDragging();
                }
                dragging = false;
                //Destroy(selectGO.GetComponent<SpringJoint>());
                //if (selectGO.transform.parent == springJoint)
                //{
                //    selectGO.transform.parent = null;
                //    selectGO = null;
                //    Destroy(springJoint);
                //    springJoint = null;
                //    Destroy(mousePoint);
                //    mousePoint = null;
                //}
                
                Destroy(line);
                line = null;
            }

            if (dragging)
            {
                if (selectGO.GetComponent<Rigidbody>() != null)
                {
                    //mousePoint.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    //    mZOffset));
                    selectGO.GetComponent<Rigidbody>().velocity =
                        (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        mZOffset)) - selectGO.transform.position) * 10;
                }
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

    void StartDragging(Vector3 hitPoint)
    {
        SpringJoint joint = selectGO.AddComponent<SpringJoint>();
        //joint.autoConfigureConnectedAnchor = false;
        mousePoint = Instantiate(mousePointPrefab);
        mousePoint.transform.position = hitPoint;
        joint.connectedBody = mousePoint.GetComponent<Rigidbody>();

        // spring values
        joint.spring = 100f;
        joint.damper = 50f;
        joint.tolerance = .01f;
        joint.massScale = 100f;

        line.GetComponent<LineController>().SetAnchor(selectGO.transform);

        mZOffset = Camera.main.WorldToScreenPoint(selectGO.transform.position).z;
    }

    void StopDragging()
    {
        Destroy(selectGO.GetComponent<SpringJoint>());
        Destroy(mousePoint);
        mousePoint = null;
    }
}
