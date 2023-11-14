using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;
//using UnityEngine.UIElements;
//using static UnityEditor.FilePathAttribute;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

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
    public static StationManager instance;
    public Location playerLocation;

    //Rect backBounds;

    public Stack<Location> playerPath;

    // Used for camera movement/tweening
    float moveProgress = 0.0f;
    bool movingCam = false;

    [SerializeField]
    float moveSpeed = 1.0f;

   
    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;

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
        if (playerPath.Count > 1)
        {
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                StationManager.instance.StationMoveBack();
            }
        }

        // Camera tweening
        if (movingCam)
        {
            // playerLocation.cameraPos is used because's player's location has already been changed internally
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, playerLocation.cameraPos, moveProgress);
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, playerLocation.cameraRotation, moveProgress);

            moveProgress += Time.deltaTime * moveSpeed;

            // Target reached, stop moving
            if(moveProgress >= 1.0f)
            {
                movingCam = false;
            }
        }

      
    }


    /// <summary>
    /// The player moves to a station upon clicking
    /// </summary>
    /// <param name="loc">The station being targeted to move to</param>
    public void MoveToStation(Location loc)
    {
        //foreach(var i in playerLocation.interactables)
        //{
        //    i.GetComponent<IHighlightable>().
        //}
        // Trigger to begin moving camera
        moveProgress = 0.0f;
        movingCam = true;

        // If station does not already exist in player's path, add it to stack
        if(!playerPath.Contains(loc))
        {
            StationManager.instance.playerPath.Push(loc);
        }
        
        // Enable collider before leaving
        if(StationManager.instance.playerLocation.clickableCollider != null)
        {
            StationManager.instance.playerLocation.clickableCollider.enabled = true;
        }

        // Update player's current location
        StationManager.instance.playerLocation = loc;

        if (loc.clickableCollider != null)
        {
            loc.clickableCollider.enabled = false;
        }

        if (playerPath.Count > 1)
        {
            GameManager.Instance.UIManager.TurnOnBackButton();
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                StationManager.instance.StationMoveBack();
            }
        }
        else
        {
            GameManager.Instance.UIManager.TurnOffBackButton();
        }
    }

    /// <summary>
    /// Moves the player to the 
    /// </summary>
    public void StationMoveBack()
    {
        if(playerLocation == ExamineManager.instance.examineStation)
        {
            ExamineManager.instance.QuitExamining();
        }

        if (StationManager.instance.playerLocation.clickableCollider != null)
        {
            StationManager.instance.playerLocation.clickableCollider.enabled = true;
        }
        playerPath.Pop();
        MoveToStation(playerPath.Peek());

    }

}
