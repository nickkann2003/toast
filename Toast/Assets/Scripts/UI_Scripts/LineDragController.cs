using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    // used for object with rigidbody
    Transform anchor;
    Vector3 mousePos;
    Vector3 mOffset;

    // used for object without rigidbody
    Vector3 anchorPoint;


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

    public void SetAnchorPoint(int i, Vector3 pos)
    {
        lineRenderer.SetPosition(i, pos);
    }

    public void SetOffset(Vector3 offset)
    {
        mOffset = offset;
    }

    // Update is called once per frame
    void Update()
    {   
        if (anchor != null)
        {
            lineRenderer.SetPosition(0, anchor.position + mOffset);
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(anchor.position).z)) + mOffset;
            lineRenderer.SetPosition(1, mousePos);
        }
    }
}
