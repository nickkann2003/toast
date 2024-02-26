using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEffect : MonoBehaviour
{

    public float fadeInDuration = 1f; // Duration of the fade in transition
    public float stayDuration = 1f; // Duration of the stay at full scale
    public float fadeOutDuration = 1f; // Duration of the fade out transition
    public AnimationCurve scaleCurve; // Animation curve for scaling

    private RectTransform rectTransform;
    private bool isTransitioning = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero; // Start with the UI hidden
    }

    public void StartTransition()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(TransitionRoutine());
        }
    }

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
        yield return new WaitForSeconds(stayDuration);

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
