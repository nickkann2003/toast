using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsingInfoManager : MonoBehaviour
{
    public GameObject UIPanel;

    public static UsingInfoManager instance;

    [SerializeField] NewHand playerHand;

    public ToastNinja toastNinja;

    [SerializeField] TextMeshProUGUI useTextUI, pickUpTextUI;

    [Header("Special Use Cases")]
    [SerializeField]
    UseEffectSO knifeUse;
    [SerializeField]
    UseEffectSO spreadUse;

    // Singleton
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInfo(NewProp prop, int num)
    {
        if(GameManager.Instance.curState != GameState.inGame)
        {
            return;
        }

        UIPanel.transform.position = Camera.main.WorldToScreenPoint(prop.transform.position); 

        UIPanel.SetActive(true);

        // Check for special cases and set text activity
        if(UseTextSpecialCases(prop))
        {
            useTextUI.gameObject.SetActive(false);
        }
        else
        {
            if(!useTextUI.gameObject.activeSelf)
            {
                useTextUI.gameObject.SetActive(true);
            }
        }

        // Check for special cases and set text activity
        if(PickUptextSpecialCases(prop))
        {
            pickUpTextUI.gameObject.SetActive(false);
        }
        else
        {
            if(!pickUpTextUI.gameObject.activeSelf)
            {
                pickUpTextUI.gameObject.SetActive(true);
            }
        }

        // If using knife...
        if (playerHand.IsHoldingItem)
        {
            if (playerHand.CheckObject().TryGetComponent(out Knife knife))
            {
                useTextUI.text = "Use Knife (<sprite=78>)";
                useTextUI.gameObject.SetActive(true);
            }
        } 
    }

    public void HideInfo(NewProp prop, int num)
    {
        UIPanel.SetActive(false);

        useTextUI.text = "Use (<sprite=78>)";
    }

    private bool UseTextSpecialCases(NewProp prop)
    {
        if(prop.UseEffectsCount <= 0)
        {
            return true;
        }

        if (prop.HasUseEffect(spreadUse) || prop.HasUseEffect(knifeUse))
        {
            return true;
        }

        if (playerHand.IsHoldingItem)
        {
            return true;
        }
            

        return false;
    }

    private bool PickUptextSpecialCases(NewProp prop)
    {
        if (playerHand.IsHoldingItem)
        {
            return true;
        }

        return false;
    }
}
