using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    private LineRenderer lineRenderer;

    // used for object with rigidbody
    private Transform anchor;
    private Vector3 mousePos;
    private Vector3 mOffset;

    // used for object without rigidbody
    private Vector3 anchorPoint;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    /// <summary>
    /// Sets the anchor point for the drag line to an objects transform
    /// </summary>
    /// <param name="objectTransform">Transform of anchor</param>
    public void SetAnchor(Transform objectTransform)
    {
        anchor = objectTransform;
    }

    /// <summary>
    /// Sets the anchor point of the line to a position in space
    /// </summary>
    /// <param name="i"></param>
    /// <param name="pos"></param>
    public void SetAnchorPoint(int i, Vector3 pos)
    {
        lineRenderer.SetPosition(i, pos);
    }

    /// <summary>
    /// Sets the mouse offset
    /// </summary>
    /// <param name="offset"></param>
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
