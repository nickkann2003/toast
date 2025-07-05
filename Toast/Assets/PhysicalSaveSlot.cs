using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;

using NaughtyAttributes;
using UnityEngine.UI;

public class PhysicalSaveSlot : MonoBehaviour
{
    private Animator ani;
    [Header("Stats")]
    [SerializeField, BoxGroup("Stats")] private Slider achievementSlider;
    [SerializeField, BoxGroup("Stats")] private Slider objectiveSlider;
    [SerializeField, BoxGroup("Stats")] private TextMeshPro breadEatenCount;
    [SerializeField, BoxGroup("Stats")] private TextMeshPro breadToastedCount;
    [SerializeField, BoxGroup("Stats")] private TextMeshPro littleFellaItemCount;

    [Header("Reference")]
    [SerializeField] private GameObject saveSlots_Bread;
    [SerializeField] private GameObject saveSlotButton_Blackframe;
    private int hoverOverTracker = 0;

    void Awake()
    {
        ani = this.GetComponent<Animator>();
    }

    void OnMouseEnter()
    {
        ChangeHoverOverTracker(1);
    }

    void OnMouseExit()
    {
        ChangeHoverOverTracker(-1);
    }

    /// <summary>
    /// This will be invoked in the animator controller as animation event
    /// </summary>
    public void MoveToMainGame(int fileId)
    {
        SaveHandler.instance.SetCurrentSaveFileByID(fileId);
        SaveHandler.instance.LoadSaveFile();
    }

    /// <summary>
    /// This will be invoked when the play button is clicked
    /// </summary>
    public void PlayBreadSlideInAnimation(int slotIndex)
    {
        ani.SetTrigger(slotIndex == 0 ? "breadToToaster_Left" : "breadToToaster_Right");
    }

    public void SetSliderStats(int curAchievementCount, int totalAchievement,
                               int curObjectiveCount, int totalObjectiveCount)
    {
        // TODO: May consider remove totalAchievement parameter once we have a final count. 
        // We can set up in the inspector
        achievementSlider.value = curAchievementCount;
        achievementSlider.maxValue = totalAchievement;

        objectiveSlider.value = curObjectiveCount;
        objectiveSlider.maxValue = totalObjectiveCount;
    }

    public void SetNumberStats(int breadEaten, int breadToasted, int littleFellaItem)
    {
        breadEatenCount.text = breadEaten.ToString();
        breadToastedCount.text = breadToasted.ToString();
        littleFellaItemCount.text = littleFellaItem.ToString();
    }

    public void ResetAllStats()
    {
        SetSliderStats(0, 1, 0, 1);
        SetNumberStats(0, 0, 0);

        hoverOverTracker = 0;
        ani?.SetInteger("hoverOver_IntTracker", hoverOverTracker);
    }

    public void NewSaveSlot(int fileIndex)
    {
        saveSlotButton_Blackframe.SetActive(false);
        saveSlots_Bread.SetActive(true);
        SaveHandler.instance.SetCurrentSaveFileByID(fileIndex);
        SaveHandler.instance.SetSaveFileName("SaveSlot");
    }

    public void SetUpExistingSaveSlot()
    {
        saveSlotButton_Blackframe.SetActive(false);
        saveSlots_Bread.SetActive(true);
    }

    /// <summary>
    /// This will be called in the physical button for "Play" / "Delete"
    /// </summary>
    /// <param name="changeAmount"></param>
    public void ChangeHoverOverTracker(int changeAmount)
    {
        hoverOverTracker += changeAmount;
        ani.SetInteger("hoverOver_IntTracker", hoverOverTracker);
    }


}
