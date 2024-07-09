using UnityEngine;

[CreateAssetMenu(fileName = "New Jam Config", menuName = "Prop/Config/Jam", order = 53)]
public class JamConfig : ScriptableObject
{
    [SerializeField]
    private Material material;
    public Material Material { get { return material; } }

    [SerializeField]
    private GameObject[] splatPrefabs;
    public GameObject SplatPrefab { get { return RandomSplatPrefab(); } }

    private GameObject RandomSplatPrefab()
    {
        return splatPrefabs[Random.Range(0, splatPrefabs.Length)];
    }
}
