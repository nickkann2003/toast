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
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompletePuzzle()
    {

    }
}
