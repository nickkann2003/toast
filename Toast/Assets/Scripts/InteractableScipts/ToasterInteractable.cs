using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterInteractable : MonoBehaviour, InteractableObject
{
    private float dialToastingValue = 0;
    private float currentToastingValue = 0;
    public void buttonDown(string inputValue)
    {
        switch (inputValue)
        {
            case "lever": break;
            case "cancel": break;
            case "dial": break;
        }
    }

    public void buttonUp(string inputValue)
    {
        switch (inputValue)
        {
            case "lever": break;
            case "cancel": break;
            case "dial": break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
