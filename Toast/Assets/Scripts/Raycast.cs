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

    GameObject line;
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
                line = Instantiate(linePrefab);
                print($"Object: \"{hitGO.name}\"");
                selectGO = hitGO;
                print(hit.point);
                line.GetComponent<LineController>().SetAnchor(selectGO.transform);
                dragging = true;


                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //cube.transform.position = hit.point;
                //hitGO.transform.parent = cube.transform;
            }
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                Destroy(line);
                line = null;
            }

            if (dragging)
            {
                //line.SetMouse(new Vector3());
            }

            // draw ray at hit point
            Debug.DrawLine(ray.origin, hit.point, Color.yellow);
            //Debug.DrawRay(hit.point, Vector3.Reflect(transform.InverseTransformPoint(hit.point), hit.normal), Color.blue);
        }
    }
}
