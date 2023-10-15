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

    public GameObject linePrefab;
    public GameObject mousePointPrefab;

    GameObject mousePoint;
    GameObject springJoint;
    GameObject line;
    float mZOffset;

    bool dragging;

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
        if (Physics.Raycast(ray, out hit))
        {
            hitGO = hit.collider.gameObject;

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
                    //mousePoint = Instantiate(mousePointPrefab);
                    //springJoint = mousePoint.transform.GetChild(0).gameObject;
                    //mousePoint.transform.position = hit.point;
                    //selectGO.transform.parent = springJoint.transform;

                    line.GetComponent<LineController>().SetAnchor(selectGO.transform);

                    mZOffset = Camera.main.WorldToScreenPoint(selectGO.transform.position).z;
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
                dragging = false;
                if (selectGO.transform.parent == springJoint)
                {
                    selectGO.transform.parent = null;
                    selectGO = null;
                    Destroy(springJoint);
                    springJoint = null;
                    Destroy(mousePoint);
                    mousePoint = null;
                }
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
    }
}
