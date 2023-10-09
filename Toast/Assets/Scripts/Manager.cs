using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.HighDefinition;
using static UnityEditor.FilePathAttribute;

public enum Stations
{
    Starting,
    Table,
    Toaster
}

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public Location playerLocation;

    //Rect backBounds;

    public Stack<Location> playerPath;

    private Stations curStation;
    private Stations prevStation;

    // Temporary public variables
    public Outline toasterOutline;
    public Outline dialOutline;
    public Outline tableOutline;
    public GameObject backButton;
    public Volume globalVolume;

    private VolumeProfile profile;
    private DepthOfField depthOfField;
   
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
        
        // Will be used once have the stations state machine
        curStation = Stations.Starting; 
        prevStation = Stations.Starting;

        profile = globalVolume.profile;
        Debug.Log(profile);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPath = new Stack<Location>();

        //backBounds = new Rect(0, 0, Screen.width, Screen.height / 10);

        MoveToStation(playerLocation);
    }

    // Update is called once per frame
    void Update()
    {
        //// If player can move out a station, do so when clicking bottom part of screen
        //if (playerPath.Count > 1)
        //{
        //    if (Input.GetMouseButtonDown(0) && backBounds.Contains(Input.mousePosition))
        //    {
        //        StationMoveBack();
        //    }
        //}

        // temporary code for MVI
        switch(curStation) 
        {
            case Stations.Starting:
                tableOutline.enabled= true;
                toasterOutline.enabled= false;
                dialOutline.enabled= false;

                backButton.SetActive(false);
                break;
            case Stations.Table:
                tableOutline.enabled = false;
                toasterOutline.enabled = true;
                dialOutline.enabled = false;

                backButton.SetActive(true);
                break;
            case Stations.Toaster:
                tableOutline.enabled = false;
                toasterOutline.enabled = false;
                dialOutline.enabled = true;
                backButton.SetActive(true);
                break;

        }

    }

    public void SetStations(Stations station)
    {
        prevStation= curStation;
        curStation= station;
    }

    /// <summary>
    /// The player moves to a station upon clicking
    /// </summary>
    /// <param name="loc">The station being targeted to move to</param>
    public void MoveToStation(Location loc)
    {
        if(!playerPath.Contains(loc))
        {
            Manager.instance.playerPath.Push(loc);
        }
        
        Manager.instance.playerLocation = loc;

        Camera.main.transform.position = loc.cameraPos;
        Camera.main.transform.rotation = loc.cameraRotation;

        if (loc.clickableCollider != null)
        {
            loc.clickableCollider.enabled = false;
        }

        if(profile.TryGet(out depthOfField)){
            if(loc != null)
            {
                Vector3 focusVector = loc.focusPos - Camera.main.transform.position;
                MinFloatParameter focusStart = new MinFloatParameter(focusVector.magnitude * 1.2f, 0f, true);
                MinFloatParameter focusEnd = new MinFloatParameter(focusVector.magnitude * 2f, 0f, true);
                Debug.Log(focusVector.magnitude);
                Debug.Log(focusStart);
                depthOfField.gaussianStart = focusStart;
                depthOfField.gaussianEnd = focusEnd;
            }
        }
    }

    /// <summary>
    /// Moves the player to the 
    /// </summary>
    public void StationMoveBack()
    {
        playerPath.Pop();
        SetStations(playerPath.Peek().stationLabel);
        MoveToStation(playerPath.Peek());
    }

}
