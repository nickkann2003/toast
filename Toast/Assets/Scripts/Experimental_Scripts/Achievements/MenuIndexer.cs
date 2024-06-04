using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuIndexer : MonoBehaviour
{
    public int index;
    public int maxIndex;
    [SerializeField] bool keyDown;
    [SerializeField] RectTransform rect;
    int verticalMovement;
    bool isPressUp, isPressDown;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
