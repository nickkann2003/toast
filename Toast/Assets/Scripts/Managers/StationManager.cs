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
    public Station playerLocation;

    //Rect backBounds;

    public Stack<Station> playerPath;

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
        playerPath = new Stack<Station>();

        //backBounds = new Rect(0, 0, Screen.width, Screen.height / 10);
        MoveToStation(playerLocation);
        moveSpeed= UIManager.instance.moveSpeedSlider.value;
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
    public void MoveToStation(Station loc)
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
        if(StationManager.instance.playerLocation.clickableCollider != null && StationManager.instance.playerLocation != loc.parentLoc)
        {
            StationManager.instance.playerLocation.clickableCollider.enabled = true;
            StationManager.instance.playerLocation.OnLeave();
        }

        // Update player's current location
        StationManager.instance.playerLocation = loc;
        StationManager.instance.playerLocation.OnArrive();


        if (loc.clickableCollider != null)
        {
            loc.clickableCollider.enabled = false;
        }

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

    /// <summary>
    /// Moves the player to the 
    /// </summary>
    public void StationMoveBack()
    {
        if(playerLocation == ExamineManager.instance.examineStation)
        {
            ExamineManager.instance.QuitExamining();
        }

        if(playerLocation == InventoryManager.instance.InventoryStation)
        {
            InventoryManager.instance.SetLeaveInventoryValues();
        }

        if (StationManager.instance.playerLocation.clickableCollider != null)
        {
            StationManager.instance.playerLocation.clickableCollider.enabled = true;
            //StationManager.instance.playerLocation.OnLeave();
        }

            // No parent location exists, do stack manipulation
            if (playerLocation.parentLoc == null)
            {
                if (playerPath.Count > 1)
                {
                    playerPath.Pop();

                    Debug.Log("MOVE TO BACK" + playerPath.Peek());
                    MoveToStation(playerPath.Peek());
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

                Debug.Log("MOVE TO PARENT" + playerLocation.parentLoc);
                MoveToStation(playerLocation.parentLoc);
            }
      
 
       

    }


    public void ChangeMoveSpeed()
    {
        moveSpeed = UIManager.instance.moveSpeedSlider.value;
    }
}
