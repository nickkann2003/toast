using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.FilePathAttribute;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    public Location playerLocation;

    Rect backBounds;

    public Stack<Location> playerPath;

    // EXTREMELY BASIC SINGLETON, SHOULD BE REPLACED LATER
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPath = new Stack<Location>();

        backBounds = new Rect(0, 0, Screen.width, Screen.height / 10);

        MoveToStation(playerLocation);
    }

    // Update is called once per frame
    void Update()
    {
        // If player can move out a station, do so when clicking bottom part of screen
        if (playerPath.Count > 1)
        {
            if (Input.GetMouseButtonDown(0) && backBounds.Contains(Input.mousePosition))
            {
                StationMoveBack();
            }
        }

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

        Debug.Log(playerPath.Count);
    }

    /// <summary>
    /// Moves the player to the 
    /// </summary>
    void StationMoveBack()
    {
        playerPath.Pop();
        MoveToStation(playerPath.Peek());
    }

}
