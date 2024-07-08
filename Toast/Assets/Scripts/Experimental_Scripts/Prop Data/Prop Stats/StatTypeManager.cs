using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTypeManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public StatType massType;
    public StatType sizeType;
    public StatType toastType;
    public StatType frozenType;

    // Singleton
    public static StatTypeManager instance;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
    }
}
