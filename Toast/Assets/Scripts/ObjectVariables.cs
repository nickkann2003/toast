using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVariables : MonoBehaviour
{
    public Object objectId;
    public List<Attribute> attributes = new List<Attribute>();
    public void AddAttribute(Attribute attribute)
    {
        if (!attributes.Contains(attribute))
        {
            attributes.Add(attribute);
        }
    }

    public void RemoveAttribute(Attribute attribute)
    {
        if (attributes.Contains(attribute))
        {
            attributes.Remove(attribute);
        }
    }
}
