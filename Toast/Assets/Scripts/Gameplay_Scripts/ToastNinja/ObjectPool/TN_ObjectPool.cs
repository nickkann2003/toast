using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New TN Object Pool", menuName = "Minigames/Toast Ninja/Object Pool", order = 55)]

public class TN_ObjectPool : ScriptableObject
{
    [SerializeField, OnValueChanged("UpdateWeights")]
    private TN_ObjectPoolItem[] _items;

    [SerializeField, ReadOnly]
    private int totalWeight;

    public GameObject RandomItem()
    {
        if (_items == null) { return null; }

        int rand = Random.Range(0, totalWeight);

        for (int i = 0; i < _items.Length; i++)
        {
            rand -= _items[i].Weight;

            if (rand < 0)
            {
                return _items[i].Object;
            }
        }

        return null;
    }

    private void UpdateWeights()
    {
        if (_items == null) { return; }

        totalWeight = 0;

        for (int i = 0; i < _items.Length; i++)
        {
            totalWeight += _items[i].Weight;
        }
    }
}

[Serializable]
public class TN_ObjectPoolItem
{
    [SerializeField]
    private GameObject ToastNinjaObject;

    [SerializeField, MinValue(1), AllowNesting]
    private int weight;

    public int Weight { get { return weight; } }

    public GameObject Object { get { return ToastNinjaObject; } }

}
