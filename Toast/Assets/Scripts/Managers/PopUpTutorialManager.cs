using System.Collections;
using System.Collections.Generic;

using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopUpTutorialManager : MonoBehaviour
{
    public static PopUpTutorialManager instance;

    [SerializeField] private List<PropSO> propList;
    [SerializeField] private List<PropSO> checkedPropList;

    [SerializeField] private Animator m_PopupTutorialAnimator;
    [SerializeField] private TMP_Text m_ItemNameText;

    [SerializeField] private GameObject m_ExamineItemUIContainer;
    [SerializeField] private TMP_Text m_ExamineItemNameText;
    [SerializeField] private TMP_Text m_ExamineItemDescriptionText;

    // [Header("Inspect Item Notification")]


    // Basic singleton
    private void Awake()
    {
        instance = this;
    }

    public void ShowPopUpTutorial(NewProp prop, int count)
    {
        m_PopupTutorialAnimator.SetTrigger("Popup");
        m_ItemNameText.text = prop.PropSO.DisplayName;
    }

    public void ShowExamineViewProp(NewProp prop, int count)
    {
        m_ExamineItemUIContainer.SetActive(true);
        m_ExamineItemNameText.text = prop.PropSO.DisplayName;
        m_ExamineItemDescriptionText.text = prop.PropSO.Description;
    }
}
