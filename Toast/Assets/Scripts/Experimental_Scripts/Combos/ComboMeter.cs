using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Author: Nick Kannenberg
// This script handles a combo meter, which has a multiplier and adjustable score variables
// Does not store a score, this is meant to be used in junction with other minigame scripts
public class ComboMeter : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField]
    private Image backgroundBar;
    [SerializeField]
    private Image foregroundBar;
    [SerializeField]
    private TextMeshProUGUI comboLabel;

    [BoxGroup("Combo Variables")]
    [SerializeField]
    private float comboDuration;
    [BoxGroup("Combo Variables")]
    [SerializeField]
    private float comboDurationMultiplier;
    
    private int currentCombo = 1;

    private float currentComboDuration = 0.0f;
    private float currentMaxComboDuration = 0.0f;

    // Properties
    public int CurrentCombo { get => currentCombo; set => currentCombo = value; }

    [Button]
    private void IncreaseComboBy1() { IncrementCombo(); }
    [Button]
    private void ResetComboTo1() { ResetCombo(); }

    // Start is called before the first frame update
    void Start()
    {
        // On start, reset combo
        ResetCombo();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is a combo, set meter, count time
        if(currentCombo > 1)
        {
            Vector3 lScale = foregroundBar.transform.localScale;
            foregroundBar.transform.localScale = new Vector3((currentComboDuration / currentMaxComboDuration), lScale.y, lScale.z);
            currentComboDuration -= Time.deltaTime;
            if(currentComboDuration < 0)
            {
                ResetCombo();
            }
        }
    }

    // Set the combo to a given value
    private void SetCombo(int newCombo)
    {
        currentCombo = newCombo;
        currentComboDuration = comboDuration * Mathf.Pow(comboDurationMultiplier, currentCombo);
        currentMaxComboDuration = currentComboDuration;
        comboLabel.SetText("Combo! " + currentCombo + "x!");
        if(newCombo == 1)
        {
            DisableVisibleChildren();
        }
        else
        {
            EnableVisibleChildren();
        }
    }

    // Returns a value multiplied by the current combo multiplier
    public int ComboMultiplier(int score)
    {
        return score * currentCombo;
    }

    // Increase the combo by 1
    public void IncrementCombo()
    {
        SetCombo(currentCombo + 1);
    }

    // Increases the combo by a given value
    public void IncreaseCombo(int increase)
    {
        SetCombo(currentCombo + increase);
    }

    // Decreases the combo by 1
    public void DecrementCombo()
    {
        SetCombo(currentCombo - 1);
    }

    // Decreases the combo by a given value
    public void DecreaseCombo(int decrease)
    {
        SetCombo(currentCombo - decrease);
    }

    // Reset the combo to 1
    public void ResetCombo()
    {
        SetCombo(1);
    }

    // Disables all visible UI elements relating to combo
    private void DisableVisibleChildren()
    {
        backgroundBar.enabled = false;
        foregroundBar.enabled = false;
        comboLabel.SetText("");
    }

    // Enables all visual UI elements relating to combo
    private void EnableVisibleChildren()
    {
        backgroundBar.enabled = true;
        foregroundBar.enabled = true;
    }
}
