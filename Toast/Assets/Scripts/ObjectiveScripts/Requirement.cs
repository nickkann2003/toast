using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Requirement : MonoBehaviour
{
    // Public variables
    public bool listening = false;
    public RequirementType type = RequirementType.EatObject;
    public PropFlags targetAttributes;
    public string goalName;
    public int goal;
    public bool alwaysListening = false;

    // Private variables
    private int current;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Check if the goal is complete
    public bool CheckComplete()
    {
        if(current == goal)
        {
            return true;
        }
        return false;
    }

    // Update the requirement with a given event
    public void UpdateRequirement(RequirementEvent e)
    {
        // If listening and correct type
        if((listening || alwaysListening) && e.type == type) // Ensure type, target, and listening
        {
            // If does not contain all necessary flags, return
            if(!(e.attributes.HasFlag(targetAttributes)))
            {
                return;
            }
            
            if (e.increase) // Increase or Decrease
            {
                current += 1;
            }
            else
            {
                current -= 1;
            }
            if(type != RequirementType.HaveObject) // If not [exact number type], don't allow overflow
            {
                if(current > goal)
                {
                    current = goal;
                }
            }
        }
    }

    // Returns a formatted string of this requirement to be displayed to the player
    new public string ToString
    {
        get
        {
            string value = "";
            if (CheckComplete())
            {
                value += "<color=#111><s><size=-2>";
                value += goalName + ": ";
                value += "DONE!";
            }
            else
            {
                value += "<color=#000><size=+0>";
                value += goalName + ": ";
                value += current + "/" + goal;
            }
            value += "</size></s></color>";
            return value;
        }
    }
}
