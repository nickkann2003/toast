using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementEvent
{
    public RequirementType type;
    public ObjectVariables targetVars;
    public bool increase;

    public RequirementEvent(RequirementType type, ObjectVariables vars, bool increase)
    {
        this.type = type;
        this.targetVars = vars;
        this.increase = increase;
    }
}
