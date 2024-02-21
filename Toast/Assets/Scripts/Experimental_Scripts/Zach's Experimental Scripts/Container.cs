using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [SerializeField]
    GameObject objPrefab;

    public GameObject GrabContent()
    {
        return objPrefab;
    }
}
