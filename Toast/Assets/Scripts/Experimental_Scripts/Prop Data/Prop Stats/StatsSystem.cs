using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatsSystem
{
    private NewProp baseProp;
    public NewProp BaseProp { get { return baseProp; } }

    private readonly Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    // PURELY FOR EDITOR
    [SerializeField]
    private List<Stat> statList = new List<Stat>();

    //public StatsSystem(BaseStats baseStats)
    //{
    //    foreach (var stat in baseStats.Stats)
    //    {
    //        stats.Add(stat.StatType, new Stat(stat.Value));
    //    }
    //}
    public StatsSystem() { }

    public Stat GetStat(StatType type)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            return null;
        }

        return stat;
    }

    public void IncrementStat(StatType type, float amount)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            stat = new Stat(type, this);

            stats.Add(type, stat);
            statList.Add(stat);
        }

        stat.IncreaseValue(amount);
    }

    public void AddStat(Stat stat)
    {
        // if the item doesn't have the stat, add it
        if (stats.TryAdd(stat.Type, stat))
        {
            statList.Add(stat);
        }
    }

    public void AddModifier(StatType type, StatModifier modifier)
    {
        // if the item doesn't have the stat, add it
        if (!stats.TryGetValue(type, out Stat stat))
        {
            stat = new Stat(type, this);

            stats.Add(type, stat);
            statList.Add(stat);
        }

        stat.AddModifier(modifier);
    }

    public void RemoveModifier(StatType type, StatModifier modifier)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            return;
        }

        stat.RemoveModifier(modifier);
    }

    public void AddConditional(StatType type, StatConditional conditional)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            return;
        }
        
        stat.AddConditional(conditional);
    }

    public void RemoveConditional(StatType type, StatConditional conditional)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            return;
        }

        stat.RemoveConditional(conditional);
    }

    public override string ToString()
    {
        string stringToReturn = "";

        if (stats.Count > 0)
        {
            for (int i = 0; i < stats.Count; i++)
            {
                var item = stats.ElementAt(i);
                stringToReturn += "\n - " + item.Value.ToString();
                //if (i + 1 < stats.Count)
                //{
                //    stringToReturn += "\n";
                //}
            }
        }

        return base.ToString() + stringToReturn;
    }

    public void SetBaseProp(NewProp newProp)
    {
        baseProp = newProp;
    }
}
