using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* https://www.youtube.com/watch?v=Gkb_tcVXHJo&list=LL&index=14&ab_channel=DapperDino */

[CreateAssetMenu(fileName = "New Base Stats", menuName = "Prop/Stats/Base Stats", order = 53)]
public class BaseStats : ScriptableObject
{
    [SerializeField]
    private Stat[] stats;

    public Stat[] Stats {  get { return stats; } }

    //[Serializable]
    //public class BaseStat
    //{
    //    [SerializeField] private StatType statType = null;
    //    [SerializeField] private float value = 0f;

    //    public StatType StatType => statType;

    //    public float Value => value;
    //}
}
