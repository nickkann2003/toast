using System.Collections;
using System.Collections.Generic;

using NaughtyAttributes;
using UnityEngine;

public class PopUpTutorialManager : MonoBehaviour
{
    public static PopUpTutorialManager instance;

    public List<PropSO> propList;
    public List<PropSO> checkedPropList;

    // [Header("Inspect Item Notification")]


    // Basic singleton
    private void Awake()
    {
        instance = this;
    }


}
