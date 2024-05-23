using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerUI : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public Image image;

    public ToastingBreadTest toasterScript;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = toasterScript.timer / toasterScript.maxTime;
    }
}
