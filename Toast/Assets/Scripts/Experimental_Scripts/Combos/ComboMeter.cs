using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboMeter : MonoBehaviour
{
    [SerializeField]
    private Image backgroundBar;
    [SerializeField]
    private Image foregroundBar;
    [SerializeField]
    private TextMeshProUGUI comboLabel;

    private int currentCombo = 1;

    [SerializeField]
    private float comboDuration;
    private float currentComboDuration = 0.0f;
    private float currentMaxComboDuration = 0.0f;
    [SerializeField]
    private float comboDurationMultiplier;

    [Button]
    private void MethodOne() { IncreaseCombo(); }

    // Start is called before the first frame update
    void Start()
    {
        ResetCombo();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void IncreaseCombo()
    {
        SetCombo(currentCombo + 1);
    }

    public void ResetCombo()
    {
        SetCombo(1);
    }

    private void DisableVisibleChildren()
    {
        backgroundBar.enabled = false;
        foregroundBar.enabled = false;
        comboLabel.SetText("");
    }

    private void EnableVisibleChildren()
    {
        backgroundBar.enabled = true;
        foregroundBar.enabled = true;
    }
}
