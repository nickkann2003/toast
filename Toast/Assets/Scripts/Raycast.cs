using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Raycast : MonoBehaviour
{
    public Camera targetCamera;

    // debug canvas stuff
    public GameObject canvasGO;
    public GameObject infoText;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        TestRaycast();
    }

    void TestRaycast()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = targetCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);
            print(hit.point);
            // draw ray at hit point
            Debug.DrawRay(ray.origin, transform.InverseTransformPoint(hit.point), Color.yellow);
        }
    }
}
