using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spread : MonoBehaviour
{
    [SerializeField]
    Color spreadColor;

    [SerializeField]
    GameObject splatterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplySpread(NewProp breadToSpread)
    {
        // Double check if prop somehow isn't bread
        if(!breadToSpread.HasFlag(PropFlags.Bread))
        {
            return;
        }

        Instantiate(splatterPrefab);
    }
}
