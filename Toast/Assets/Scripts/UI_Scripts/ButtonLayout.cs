using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLayout : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public Vector2 newPosition;

    // ------------------------------- Functions -------------------------------
    void Awake()
    {
        // Call the method to adjust the button position based on the input position
        AdjustButtonPosition();
    }

    /// <summary>
    /// Adjusts the buttons position 
    /// </summary>
    void AdjustButtonPosition()
    {
        RectTransform buttonTransform = GetComponent<RectTransform>();

        if (buttonTransform != null)
        {
            // Adjust the button position based on the input position
            buttonTransform.anchoredPosition = newPosition;
        }
        else
        {
            Debug.LogError("ButtonPositionAdjuster script requires a RectTransform component on the GameObject.");
        }
    }
}
