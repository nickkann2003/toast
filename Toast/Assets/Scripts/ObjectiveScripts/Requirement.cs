using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Requirement : MonoBehaviour
{
    // Public variables
    public bool listening = false;
    public RequirementType type = RequirementType.EatObject;
    public List<Object> targetObjects;
    public List<Attribute> targetAttributes;
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
        if((listening || alwaysListening) && e.type == type && targetObjects.Contains(e.targetVars.objectId)) // Ensure type, target, and listening
        {
            foreach(Attribute a in targetAttributes) // Ensure all necessary attributes are present
            {
                if (!e.targetVars.attributes.Contains(a)) // If the target's att's did not contain all our require att's, return
                {
                    return;
                }
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

    new public string ToString
    {
        get
        {
            string value = "";
            value = goalName + ": " + current + "/" + goal;
            if (CheckComplete())
            {
                value += " -- DONE!";
            }
            return value;
        }
    }
}
