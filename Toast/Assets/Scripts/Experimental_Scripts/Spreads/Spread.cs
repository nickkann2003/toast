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

    public bool IsOnKnife;
    public Knife currentKnife;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when the spread is used to place the visual of the spread
    /// </summary>
    /// <param name="breadToSpread">The piece of bread the spread is being applied to</param>
    /// <param name="dot">The dot product of the bread's forward and the raycast hit</param>
    public void ApplySpread(NewProp breadToSpread, float dot)
    {
        // Double check if prop somehow isn't bread
        if (!breadToSpread.HasFlag(PropFlags.Bread))
        {
            return;
        }

        // Bread already has spread, return
        if(breadToSpread.HasAttribute(StatAttManager.instance.hasSpreadAtt))
        {
            return;
        }

        // Create the splatter, probably will be changed later when splatter is replaced
        GameObject obj = Instantiate(splatterPrefab);

        // Remove toast ninja fade component from splatter
        if (obj.TryGetComponent<TN_Splat>(out TN_Splat tnPart))
        {
            tnPart.enabled = false;
        }

        // Align the splatter texture
        obj.transform.localScale = new Vector3(breadToSpread.transform.localScale.x * 0.35f, breadToSpread.transform.localScale.y * 0.5f, breadToSpread.transform.localScale.z * 0.35f);
        obj.transform.rotation = breadToSpread.transform.rotation;
        obj.transform.parent = breadToSpread.transform;

        // Determine which side is up, put spread on that one
        if (dot >= 0)
        {
            obj.transform.GetChild(0).Rotate(new Vector3(0, 180, 0), Space.Self);
            obj.transform.localPosition = new Vector3(0, 0, 0.03f * breadToSpread.transform.localScale.z);
        }
        else
        {
            obj.transform.localPosition = new Vector3(0, 0, -0.03f * breadToSpread.transform.localScale.z);
        }
        
        // Set the color
        obj.GetComponentInChildren<Renderer>().material.color = spreadColor;

        // Mark that bread has spread
        breadToSpread.AddAttribute(StatAttManager.instance.hasSpreadAtt);

    }
}
