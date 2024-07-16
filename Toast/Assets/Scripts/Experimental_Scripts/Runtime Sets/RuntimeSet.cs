using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [SerializeField]
    protected List<T> items = new List<T>();
    public List<T> Items {  get { return items; } }
    public void Add(T t)
    {
        if (!items.Contains(t))
        {
            items.Add(t);
        }
    }

    public void Remove(T t)
    {
        if (items.Contains(t))
        {
            items.Remove(t);
        }
    }

    public bool Contains(T t)
    {
        return items.Contains(t);
    }

    public int Count()
    {
        return items.Count;
    }

    public void Clear()
    {
        items.Clear();
    }
}
