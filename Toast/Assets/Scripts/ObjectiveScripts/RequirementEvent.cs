using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequirementEvent
{
    // ------------------------------- Variables -------------------------------
    public RequirementType type;
    public PropFlags attributes;
    public bool increase;

    // ------------------------------- Functions -------------------------------
    public RequirementEvent(RequirementType type, PropFlags attributes, bool increase)
    {
        this.type = type;
        this.attributes = attributes;
        this.increase = increase;
    }
}
