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

    private GameObject prevGO;
    private GameObject hitGO;

    // Start is called before the first frame update
    void Start()
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

            // draw ray at hit point
            Debug.DrawRay(ray.origin, transform.InverseTransformPoint(hit.point), Color.yellow);
        }
    }
}
