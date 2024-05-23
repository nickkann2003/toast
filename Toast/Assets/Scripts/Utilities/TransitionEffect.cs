using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("-------------- Transition Variables ---------------")]
    public float fadeInDuration = 1f; // Duration of the fade in transition
    public float stayDuration = 1f; // Duration of the stay at full scale
    public float fadeOutDuration = 1f; // Duration of the fade out transition
    public AnimationCurve scaleCurve; // Animation curve for scaling
    public float stayDurationOnFirstTransition = 1f;

    private bool hadFirstTransition = false;
    private RectTransform rectTransform;
    private bool isTransitioning = false;

    // ------------------------------- Functions -------------------------------
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero; // Start with the UI hidden
    }

    /// <summary>
    /// Starts the transition effect
    /// </summary>
    public void StartTransition()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionRoutine());
        }
    }

    /// <summary>
    /// Performs the transition effect routine
    /// </summary>
    /// <returns>Enumerator for the routine</returns>
    private IEnumerator TransitionRoutine()
    {
        // Fade in transition (0 to 1)
        float timer = 0f;
        while (timer < fadeInDuration)
        {
            float scale = scaleCurve.Evaluate(timer / fadeInDuration);
            rectTransform.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = Vector3.one; // Ensure the final value is set correctly

        // Wait for the stay duration
        float additionalTime = 0f;
        if (!hadFirstTransition)
        {
            additionalTime = stayDurationOnFirstTransition - stayDuration;
            hadFirstTransition = true;
        }
        yield return new WaitForSeconds(stayDuration + additionalTime);

        // Fade out transition (1 to 0)
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            float scale = scaleCurve.Evaluate(1f - (timer / fadeOutDuration));
            rectTransform.localScale = Vector3.one * scale;
            timer += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = Vector3.zero; // Ensure the final value is set correctly

        isTransitioning = false; // Transition complete
    }
}
