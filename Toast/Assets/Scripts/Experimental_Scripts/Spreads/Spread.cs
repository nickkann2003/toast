using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        // Remove toast ninja component
        if(splatterPrefab.TryGetComponent<TN_Splat>(out TN_Splat tnPart))
        {
            tnPart.enabled = false;
        }
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

        Debug.Log("Applying Spread...");

        GameObject obj = Instantiate(splatterPrefab);
        obj.transform.localScale = breadToSpread.transform.localScale * 0.35f;
        obj.transform.parent = breadToSpread.transform;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localPosition = new Vector3(0, 0, 0.03f * breadToSpread.transform.localScale.z);
        obj.GetComponentInChildren<Renderer>().material.color = spreadColor;

    }
}
