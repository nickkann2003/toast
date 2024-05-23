using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Type", menuName = "Prop/Stats/Stat Type", order = 53)]
public class StatType : ScriptableObject /*IStatType*/
{
    [SerializeField] private new string name = "New Stat Type Name";
    [SerializeField] private float defaultValue = 0f;

    public string Name => name;
    public float DefaultValue => defaultValue;
}
