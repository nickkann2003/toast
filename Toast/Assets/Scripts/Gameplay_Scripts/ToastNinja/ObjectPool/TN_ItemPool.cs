using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New TN Item Pool", menuName = "Minigames/Toast Ninja/Item Pool", order = 55)]

public class TN_ItemPool : ScriptableObject
{
    [SerializeField, OnValueChanged("UpdateWeights")]
    private TN_ItemPoolItem[] _items;

    [SerializeField, ReadOnly]
    private int totalWeight;

    public TN_ItemScriptableObject RandomItem()
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
public class TN_ItemPoolItem
{
    [SerializeField]
    private TN_ItemScriptableObject ToastNinjaObject;

    [SerializeField, MinValue(1), AllowNesting]
    private int weight;

    public int Weight { get { return weight; } }

    public TN_ItemScriptableObject Object { get { return ToastNinjaObject; } }

}
