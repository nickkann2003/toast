/*
 * Puzzle script - Nick Kannenberg
 * 
 * Puzzle script, meant to hold all data any puzzle might need
 * 
 * Puzzles need:
 * A goal 
 *      Comprised of other objects in the scene, and completed by calling the CompletePuzzle event
 *      Each puzzle will only have a single goal. To have a chain of puzzles, have them activate each other
 *      
 * Completion Triggers
 *      A collection of things that get triggered upon completion, including:
 *          Unity Event
 *          
 * A list of puzzle specific props and their respawn info
 * 
 * A list of objects in the scene to activate when the puzzle activates, and deactivate when the puzzle is over
 * 
 * The ability to stop the player from leaving while the puzzle is active, and restore their path once the puzzle is over
 * 
 * A main puzzle overview station
 * 
 * A collection of events that play when the event is visited or left
 * 
 */

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class Puzzle : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    // Goals Variables
    private bool complete;

    // Completion Triggers
    [SerializeField]
    private UnityEvent completionEvent;

    // Puzzle Props
    private List<ObjectRespawner> puzzleRespawners = new List<ObjectRespawner>();

    // Scene Objects
    private List<GameObject> puzzleVolumes = new List<GameObject>();
    private List<GameObject> environmentPieces = new List<GameObject>();

    // Puzzle station
    [SerializeField]
    private Station puzzleStation;

    // Player Path Locking
    [SerializeField]
    private bool lockPlayerPath;
    private Station[] playerPath = new Station[50];

    // Visit events
    [SerializeField]
    private bool hasVisitEvents = false;
    [SerializeField, ShowIf("hasVisitEvents")]
    private UnityEvent visitEvents;

    // Leave events
    [SerializeField]
    private bool hasLeaveEvents = false;
    [SerializeField, ShowIf("hasLeaveEvents")]
    private UnityEvent leaveEvents;

    public bool Complete { get => complete; set => complete = value; }

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        puzzleStation = GetComponent<Station>();

        foreach (ObjectRespawner r in puzzleRespawners)
        {
            if (r != null)
            {
                r.SetSpawnParent(gameObject);
            }
        }
    }

    /// <summary>
    /// Marks the puzzle as complete and runs one-shot completion effects
    /// </summary>
    public void CompletePuzzle()
    {
        if (!complete)
        {
            completionEvent.Invoke();
            complete = true;
        }
    }

    /// <summary>
    /// Starts the puzzle, and runs one-shot starting effects
    /// </summary>
    public void StartPuzzle()
    {
        gameObject.SetActive(true);

        if (lockPlayerPath)
        {
            StationManager.instance.playerPath.CopyTo(playerPath, 0);
            StationManager.instance.playerPath.Clear();
        }

        StationManager.instance.MoveToStation(puzzleStation);
        visitEvents.Invoke();
    }

    /// <summary>
    /// Stops the puzzle, and resets puzzle values
    /// </summary>
    public void StopPuzzle()
    {
        gameObject.SetActive(false);

        if (lockPlayerPath)
        {
            for (int i = playerPath.Length - 1; i >= 0; i--)
            {
                if(playerPath[i] != null)
                {
                    StationManager.instance.playerPath.Push(playerPath[i]);
                }
            }
        }
        leaveEvents.Invoke();
    }
}
