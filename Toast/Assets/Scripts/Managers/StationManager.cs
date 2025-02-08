using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;
//using UnityEngine.UIElements;
//using static UnityEditor.FilePathAttribute;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

// ------------------------------- Enums -------------------------------
// Station enum, used to track which station it is in
public enum Stations
{
    MainMenu,
    Defualt,
    Table,
    Toaster,
    Fridge
}
public class StationManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public static StationManager instance;
    public Station playerLocation;

    public Stack<Station> playerPath;

    // Transition Effect
    public TransitionEffect transitionMask;

    // Used for camera movement/tweening
    private float moveProgress = 0.0f;
    private bool movingCam = false;

    [SerializeField]
    private float moveSpeed = 1.0f;

    private bool stationMovementLocked = false;

    // ------------------------------- Functions -------------------------------
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        playerPath = new Stack<Station>();

        //backBounds = new Rect(0, 0, Screen.width, Screen.height / 10);
        MoveToStation(playerLocation);
        moveSpeed = UIManager.instance.moveSpeedSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPath.Count > 1)
        {
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                StationManager.instance.StationMoveBack();
            }
        }

        // Camera tweening
        if (true)
        {
            // playerLocation.cameraPos is used because's player's location has already been changed internally
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, playerLocation.camPosWorldCoords(), moveProgress);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, playerLocation.camRotWorldCoords(), moveProgress);

            moveProgress += Time.deltaTime * moveSpeed;

            // Target reached, stop moving
            if (moveProgress >= 1.0f)
            {
                movingCam = false;
            }
        }


    }


    /// <summary>
    /// The player moves to a station upon clicking
    /// </summary>
    /// <param name="loc">The station being targeted to move to</param>
    public void MoveToStation(Station loc, bool forwards = true, bool disableMoveBackwards = false)
    {
        if (!stationMovementLocked)
        {
            // Trigger to begin moving camera
            moveProgress = 0.0f;
            movingCam = true;

            // If station does not already exist in player's path, add it to stack
            if (!playerPath.Contains(loc))
            {
                StationManager.instance.playerPath.Push(loc);
            }

            // Enable collider before leaving
            if (StationManager.instance.playerLocation != null)
            {
                if (StationManager.instance.playerLocation.clickableCollider != null && StationManager.instance.playerLocation != loc.parentLoc)
                {
                    if (instance.playerLocation != loc.parentLoc)
                    {
                        StationManager.instance.playerLocation.EnableColliders();
                    }

                    StationManager.instance.playerLocation.OnLeave();
                }
                else if (StationManager.instance.playerLocation.clickableCollider && StationManager.instance.playerLocation.runLeaveWhenGoingToChildren)
                {
                    if (instance.playerLocation != loc.parentLoc)
                    {
                        StationManager.instance.playerLocation.EnableColliders();
                    }

                    StationManager.instance.playerLocation.OnLeave();
                }
            }

            if (disableMoveBackwards)
            {
                playerPath.Clear();
            }

            // Update player's current location
            StationManager.instance.playerLocation = loc;
            StationManager.instance.playerLocation.OnArrive(forwards);


            loc.DisableColliders();

            if (playerPath.Count > 1)
            {
                GameManager.Instance.UIManager.BackButtonPopup();
                if (Keyboard.current.downArrowKey.wasPressedThisFrame)
                {
                    StationManager.instance.StationMoveBack();
                }
            }
            else
            {
                GameManager.Instance.UIManager.BackButtonPopdown();
            }
        }
    }

    public void MoveToStationThroughParents(Station s)
    {
        while (playerPath.Count > 1)
        {
            StationMoveBack();
        }
        MoveToStationRecursive(s);
    }

    private void MoveToStationRecursive(Station s)
    {
        if (s.parentLoc != null)
        {
            MoveToStationRecursive(s.parentLoc);
        }
        MoveToStation(s);
    }

    /// <summary>
    /// Moves the player to the 
    /// </summary>
    public void StationMoveBack()
    {
        if (!stationMovementLocked)
        {
                if (playerLocation == ExamineManager.instance.examineStation)
                {
                    ExamineManager.instance.QuitExamining();
                }
    
                StationManager.instance.playerLocation.EnableColliders();
    
    
            // No parent location exists, do stack manipulation
            if (playerLocation.parentLoc == null)
            {
                if (playerPath.Count > 1)
                {
                    playerPath.Pop();
                    MoveToStation(playerPath.Peek(), false);
                }
                   
    
            }
            // Move back to parent
            else
            {
                while (playerPath.Count > 0 &&
                    playerPath.Peek() != playerLocation.parentLoc)
                {
                    playerPath.Pop();
                }
                MoveToStation(playerLocation.parentLoc, false);
            }

        }
        
    }

    /// <summary>
    /// Changes the station movement speed, grabbing it from the UI element
    /// </summary>
    public void ChangeMoveSpeed()
    {
        moveSpeed = UIManager.instance.moveSpeedSlider.value;
    }

    /// <summary>
    /// Locks the ability to move between stations
    /// </summary>
    public void LockStationMovement()
    {
        stationMovementLocked = true;
    }

    /// <summary>
    /// Unlocks station movement, allowing movement between stations
    /// </summary>
    public void UnlockStationMovement()
    {
        stationMovementLocked = false;
    }
}
