using UnityEngine;

[CreateAssetMenu(fileName = "New TN Effect", menuName = "Prop/Use Effect/TN", order = 53)]
public class US_TN : UseEffect
{
    [SerializeField]
    private int points;
    [SerializeField]
    private IntGameEvent onDestroy;

    public override void Use(NewProp prop)
    {
        if (onDestroy == null) return;

        onDestroy.RaiseEvent(points);
    }
}
