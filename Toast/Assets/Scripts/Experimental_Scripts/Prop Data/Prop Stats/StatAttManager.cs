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

    public PropAttributeSO onFireAtt;
    public PropAttributeSO frozenAtt;
    public PropAttributeSO inHandAtt;
    public PropAttributeSO eatenAtt;

    [HorizontalLine(color: EColor.Gray)]
    public PropAttributeSO hasSpreadAtt;

    [HorizontalLine(color: EColor.Gray)]
    [Header("Spread Attributes")]
    public PropAttributeSO avocadoSpreadAtt;
    public PropAttributeSO butterSpreadAtt;


    // Singleton
    public static StatAttManager instance;

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
    }
}
