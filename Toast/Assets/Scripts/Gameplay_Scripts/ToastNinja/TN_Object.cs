using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TN_Object : MonoBehaviour, IUseStrategy
{
    [SerializeField]
    float points;
    [SerializeField]
    GameObject splatter;
    [SerializeField]
    GameObject pointObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        GameObject obj = Instantiate(splatter, transform.position, transform.rotation);
        obj.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
        ObjectiveManager.instance.UpdateObjectives(new RequirementEvent(RequirementType.ToastNinjaScore, gameObject.GetComponent<NewProp>().attributes, true));

        GameObject pointsObj = Instantiate(pointObject, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
        pointsObj.GetComponent<TextMeshPro>().color = this.GetComponent<Renderer>().material.color;
        pointsObj.GetComponent<TextMeshPro>().text = "";
        if (points >= 0)
        {
            pointsObj.GetComponent<TextMeshPro>().text += "+";
        }

        pointsObj.GetComponent<TextMeshPro>().text += points;

        Destroy(this.gameObject);
    }
}
