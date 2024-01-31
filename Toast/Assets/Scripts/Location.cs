using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Location : Station
{
    [SerializeField] private UnityEvent arrive;
    [SerializeField] private UnityEvent leave;

    public Vector3 cameraPos;
    public Quaternion cameraRotation;
    public Stations stationLabel;
    public Vector3 objectOffset;
    public Location parentLoc;

    // List of objects that is highlightables/interactable
    public List<IHighlightable> highlightables;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    private void OnMouseDown()
    {
        if(StationManager.instance!= null && StationManager.instance.playerLocation!=null)
        {
            StationManager.instance.MoveToStation(this);
            if (StationManager.instance.playerLocation.interactables != null)
            {
                if (StationManager.instance.playerLocation.interactables.Contains(this))
                {
                    StationManager.instance.MoveToStation(this);
                }
            }
        }
    }

    public void OnArrive()
    {
        arrive.Invoke();
    }

    public void OnLeave()
    {
        leave.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(cameraPos, 0.1f);
        Gizmos.DrawFrustum(cameraPos, Camera.main.fieldOfView, Camera.main.farClipPlane, Camera.main.nearClipPlane, Camera.main.aspect);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(objectOffset, 0.1f);
    }

}
