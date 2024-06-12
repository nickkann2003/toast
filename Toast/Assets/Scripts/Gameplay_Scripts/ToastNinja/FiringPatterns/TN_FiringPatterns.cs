using NaughtyAttributes;
using UnityEngine;

public abstract class TN_FiringPatterns : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [SerializeField]
    protected TN_ObjectPool _objectPool;

    [SerializeField, MinMaxSlider(0, 10), Label("Min/Max Indices")]
    protected Vector2Int minMaxIndices = new Vector2Int(0, 10);

    // ------------------------------- Properties -------------------------------

    protected int Min
    {
        get { return minMaxIndices.x; }
    }

    protected int Max
    {
        get { return minMaxIndices.y; }
    }

    // ------------------------------- Functions -------------------------------

    public abstract void Launch(ToastNinja toastNinja);

    protected virtual bool ValidateIndex(int index)
    {
        if (index < Min || index > Max)
        {
            return false;
        }

        return true;
    }

    protected virtual GameObject RandomPrefab()
    {
        return _objectPool.RandomItem();
    }
}
