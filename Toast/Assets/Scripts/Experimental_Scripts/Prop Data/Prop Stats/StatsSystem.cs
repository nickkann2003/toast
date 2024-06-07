using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSystem
{
    private readonly Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    public StatsSystem(BaseStats baseStats)
    {
        foreach (var stat in baseStats.Stats)
        {
            stats.Add(stat.StatType, new Stat(stat.Value));
        }
    }

    public void IncrementStat(StatType type, float amount)
    {
        if (!stats.TryGetValue(type, out Stat stat))
        {
            stat = new Stat(type);

            stats.Add(type, stat);
        }

        stat.UpdateBaseValue(amount);
    }

    public void AddModifier(StatType type, StatModifier modifier)
    {
        // if the item doesn't have the stat, add it
        if (!stats.TryGetValue(type, out Stat stat))
        {
            stat = new Stat(type);

            stats.Add(type, stat);
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
}
