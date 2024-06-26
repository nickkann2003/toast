using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TN_Object : MonoBehaviour, IUseStrategy
{
    // ------------------------------- Variables -------------------------------
    [Header("Variables")]
    [SerializeField]
    float points;
    [SerializeField]
    GameObject splatter;
    [SerializeField]
    GameObject pointObject;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent toastNinjaScoreEvent;

    // ------------------------------- Functions -------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Use for TNObject destroys the object and grants points
    public void Use()
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);

        GameObject obj = Instantiate(splatter, transform.position, transform.rotation);
        obj.GetComponent<Renderer>().material.color = this.GetComponent<Renderer>().material.color;
        obj.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30)*2), Space.Self);
        toastNinjaScoreEvent.RaiseEvent(gameObject.GetComponent<NewProp>(), (int)points);

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
