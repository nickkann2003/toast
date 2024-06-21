using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Item", menuName = "Minigames/Toast Ninja/Item", order = 55)]
public class TN_ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private bool isBomb = false;

    [SerializeField]
    private GameObject prefab;


    public bool IsBomb {  get { return isBomb; } }
    public GameObject Prefab { get { return prefab; } }
}
