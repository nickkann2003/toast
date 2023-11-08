using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : Station
{
    public Vector3 cameraPos;
    public Quaternion cameraRotation;
    public Stations stationLabel;
    public float objectOffset = 5f;

    // List of objects that is highlightables/interactable
    public List<IHighlightable> highlightables;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    private void OnMouseDown()
    {
        if(StationManager.instance!= null && StationManager.instance.playerLocation!=null)
        {
            if (StationManager.instance.playerLocation.interactables != null)
            {
                if (StationManager.instance.playerLocation.interactables.Contains(this))
                {
                    StationManager.instance.MoveToStation(this);
                }
            }
        }
    
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraPos, 0.1f);
        Gizmos.DrawFrustum(cameraPos, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);
    }

}
