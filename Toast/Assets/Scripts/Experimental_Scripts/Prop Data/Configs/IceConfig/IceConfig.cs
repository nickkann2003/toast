using UnityEngine;

[CreateAssetMenu(fileName = "New Ice Config", menuName = "Prop/Config/Ice", order = 53)]
public class IceConfig : ScriptableObject
{
    [SerializeField]
    private Vector3 scale = Vector3.one;
    [SerializeField]
    private Vector3 offset = Vector3.zero;

    public Vector3 Scale {  get { return scale; }  }
    public Vector3 Offset { get { return offset; } }
}
