using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : Station
{
    public Vector3 cameraPos;
    public Quaternion cameraRotation;
    public Stations stationLabel;

    // List of objects that is highlightables/interactable
    public List<IHighlightable> highlightables;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    private void OnMouseDown()
    {
        if (StationManager.instance.playerLocation.interactables.Contains(this))
        {
            StationManager.instance.MoveToStation(this);
        }
    }

   
}
