using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : Station
{
    public Vector3 cameraPos;
    public Quaternion cameraRotation;
    public Stations stationLabel;

    // List of props and stations that can be reached from here
    public List<Station> interactables;

    private void OnMouseDown()
    {
        if (Manager.instance.playerLocation.interactables.Contains(this))
        {
            Manager.instance.SetStations(stationLabel);
            Manager.instance.MoveToStation(this);
        }
    }
}