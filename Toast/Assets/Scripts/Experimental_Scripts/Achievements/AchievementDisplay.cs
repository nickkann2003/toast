using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

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

    [SerializeField]
    public Sprite lockedSprite, unlockedSprite;

    public UnityEngine.UI.Image displayImage;

    public string NameText
    {
        get { return nameText.text; }
        set { nameText.text = value; }
    }

    public string DescriptionText
    {
        get { return descriptionText.text; }
        set { descriptionText.text = value; }
    }

    public string ProgressText
    {
        get { return progressText.text; }
        set { progressText.text = value; }
    }

    public bool IsHiddenAchievement { get { return isHiddenAchievement; } }


    // Start is called before the first frame update
    void Start()
    {
        // Get reference to attached image 
        displayImage = GetComponent<UnityEngine.UI.Image>();

        // Make sure achievement is hooked up
        if(associatedAchievement != null)
        {
            // Set sprite depending on unlock status
            if(associatedAchievement.IsUnlocked)
            {
                displayImage.sprite = unlockedSprite;
            }
            else
            {
                displayImage.sprite = lockedSprite;
            }

            // Set text based on hidden
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

            // Display goal progress if applicable
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
