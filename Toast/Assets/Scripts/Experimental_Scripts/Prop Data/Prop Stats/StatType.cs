using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* https://www.youtube.com/watch?v=Gkb_tcVXHJo&list=LL&index=14&ab_channel=DapperDino */

[CreateAssetMenu(fileName = "New Stat Type", menuName = "Prop/Stats/Stat Type", order = 53)]
public class StatType : ScriptableObject /*IStatType*/
{
    [SerializeField] private new string name = "New Stat Type Name";
    [SerializeField] private float defaultValue = 0f;

    public string Name => name;
    public float DefaultValue => defaultValue;
}
