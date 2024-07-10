using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Effect", menuName = "Prop/Use Effect/TN", order = 53)]
public class USE_TN : UseEffectSO
{
    [SerializeField]
    private int points;
    [SerializeField]
    private GameObject splatter;
    [SerializeField]
    private GameObject pointObject;

    // events
    [Header("Event References")]
    [SerializeField]
    private bool invokeEvents;
    [SerializeField, EnableIf("invokeEvents")]
    private PropIntGameEvent toastNinjaScoreEvent;

    public override bool TryUse(NewProp newProp)
    {
        AudioManager.instance.PlayOneShotSound(AudioManager.instance.eatingBread);
        //if (onDestroy == null) return;

        Transform trans = newProp.transform;

        GameObject obj = Instantiate(splatter, trans.position, trans.rotation);
        obj.GetComponent<Renderer>().material.color = trans.GetComponent<Renderer>().material.color;
        obj.transform.Rotate(new Vector3(0, 0, Random.Range(-30, 30) * 2), Space.Self);
        toastNinjaScoreEvent.RaiseEvent(newProp, (int)points);

        GameObject pointsObj = Instantiate(pointObject, new Vector3(trans.position.x, trans.position.y, trans.position.z - 1), Quaternion.identity);
        pointsObj.GetComponent<TextMeshPro>().color = trans.GetComponent<Renderer>().material.color;
        pointsObj.GetComponent<TextMeshPro>().text = "";
        if (points >= 0)
        {
            pointsObj.GetComponent<TextMeshPro>().text += "+";
        }

        pointsObj.GetComponent<TextMeshPro>().text += points;

        Destroy(newProp.gameObject);

        return true;
    }
}
