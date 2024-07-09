using NaughtyAttributes;
using UnityEngine;

public class StatAttManager : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    public StatType massType;
    public StatType sizeType;
    public StatType toastType;
    public StatType frozenType;

    [HorizontalLine(color: EColor.Gray)]

    public PropAttributeSO inHandAtt;

    // Singleton
    public static StatAttManager instance;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
    }
}
