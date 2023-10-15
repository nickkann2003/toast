using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform anchor;
    Vector3 mousePos;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    public void SetAnchor(Transform objectTransform)
    {
        anchor = objectTransform;
    }

    //public void SetAnchor(Vector3 pos)
    //{
    //    lineRenderer.SetPosition(1, pos);
    //}

    // Update is called once per frame
    void Update()
    {   
        if (anchor != null)
        {
            lineRenderer.SetPosition(0, anchor.position);
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(anchor.position).z));
            lineRenderer.SetPosition(1, mousePos);
        }
    }
}
