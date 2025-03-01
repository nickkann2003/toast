using System.Linq;
using System.Collections;
using System.Collections.Generic;

using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class PopUpTutorialManager : MonoBehaviour
{
    public static PopUpTutorialManager instance;

    [SerializeField] private List<PropSO> propList;
    [SerializeField] private List<int> checkedPropListIndex;
    [SerializeField] private List<PropSO> checkedPropList;

    [SerializeField] private Animator m_PopupTutorialAnimator;
    [SerializeField] private TMP_Text m_ItemNameText;

    [SerializeField] private GameObject m_ExamineItemUIContainer;
    [SerializeField] private TMP_Text m_ExamineItemNameText;
    [SerializeField] private TMP_Text m_ExamineItemDescriptionText;

    // [Header("Inspect Item Notification")]
    // ------------------------------- Save Variables -------------------------------
    private string propSeparator = "~";
    private string spacer = "_";


    // Basic singleton
    private void Awake()
    {
        instance = this;

    }

    public void ShowPopUpTutorial(NewProp prop, int count)
    {
        if (prop == null || prop.PropSO == null)
            return;
        if(checkedPropListIndex.Contains(prop.PropSO.PropID))
            return;

        m_PopupTutorialAnimator.SetTrigger("Popup");
        m_ItemNameText.text = prop.PropSO.DisplayName;
        checkedPropListIndex.Add(prop.PropSO.PropID);
    }

    public void ShowExamineViewProp(NewProp prop, int count)
    {
        m_ExamineItemUIContainer.SetActive(true);
        m_ExamineItemNameText.text = prop.PropSO.DisplayName;
        m_ExamineItemDescriptionText.text = prop.PropSO.Description;
    }

    public string GetTutorialPopUpSaveString()
    {
        return string.Join("~", checkedPropListIndex);
    }

    public void LoadCheckedPropSaveString(string data)
    {
        if(data.Length == 0)
            return;
        
        Debug.Log(data);
        checkedPropListIndex = data.Split(propSeparator)
                                    .Select(int.Parse) // Convert each substring to an integer
                                    .ToList();
    }
}
