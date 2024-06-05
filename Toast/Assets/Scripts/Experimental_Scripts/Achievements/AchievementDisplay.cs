using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AchievementDisplay : MonoBehaviour
{
    public Achievement associatedAchievement;
    public string achievementName;
    public string description;
    public string progress;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    bool isUnlocked;
    [SerializeField]
    bool isHiddenAchievement;

    [SerializeField]
    TextMeshProUGUI nameText, descriptionText, progressText;


    // Start is called before the first frame update
    void Start()
    {
        if(associatedAchievement != null)
        {
            if (isHiddenAchievement)
            {
                nameText.text = "???";
                descriptionText.text = "???";
            }
            else
            {
                nameText.text = associatedAchievement.AchievementName;
                descriptionText.text = associatedAchievement.Description;
            }

            if (associatedAchievement.HasNumericGoal)
            {
                if(isHiddenAchievement)
                {
                    progressText.text = "?/?";
                }
                else
                {
                    progressText.text = $"{associatedAchievement.AchievementProgress}/{associatedAchievement.AchievementGoal}";
                }
                
            }
            else
            {
                progressText.text = string.Empty;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
