using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New TN Item", menuName = "Minigames/Toast Ninja/Item", order = 55)]
public class TN_ItemScriptableObject : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private bool isBomb = false;

    [SerializeField]
    private int basePoints;
    [SerializeField]
    private int pointIncreaseOnHit;

    [SerializeField, MinValue(1)]
    private int hitsToDestroy;

    [SerializeField]
    private GameObject onDestroyParticles;
    [SerializeField]
    private GameObject pointsObject;
    [SerializeField]
    private Color pointsColor;

    [SerializeField]
    private ToastNinjaScore toastNinjaScore;

    [Header("Event References")]
    [SerializeField]
    private PropIntGameEvent toastNinjaScoreEvent;

    public GameObject Prefab { get { return prefab; } }
    public bool IsBomb { get { return isBomb; } }
    public int BasePoints { get {  return basePoints; } }
    public int PointIncreaseOnHit { get {  return pointIncreaseOnHit; } }
    public int HitsToDestroy { get { return hitsToDestroy; } }
    public ToastNinjaScore ToastNinjaScore { get { return toastNinjaScore; } }

    public void SpawnPoints(Vector3 location, int hitsTaken)
    {
        int points = basePoints + (hitsTaken - 1) * pointIncreaseOnHit;

        GameObject pointsObj = Instantiate(pointsObject, location, Quaternion.identity);
        pointsObj.GetComponent<TextMeshPro>().color = pointsColor;
        pointsObj.GetComponent<TextMeshPro>().text = "";
        if (points >= 0)
        {
            pointsObj.GetComponent<TextMeshPro>().text += "+";
        }
        pointsObj.GetComponent<TextMeshPro>().text += points;
        pointsObj.transform.localScale = Vector3.one * .6f * Mathf.Abs(points);

        toastNinjaScore.AddPoints(points);
    }

    public void SpawnDestroyParticles(Vector3 location)
    {
        GameObject particleObj = Instantiate(onDestroyParticles);
        particleObj.transform.position = location;
    }

    public int GetPoints(int hitsTaken)
    {
        return basePoints + (hitsTaken - 1) * pointIncreaseOnHit;
    }
}
