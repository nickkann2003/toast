using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementEvent
{
    public RequirementType type;
    public string targetObject;
    public bool increase;

    public RequirementEvent(RequirementType type, string targetObject, bool increase)
    {
        this.type = type;
        this.targetObject = targetObject;
        this.increase = increase;
    }
}
