using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/* https://github.com/UnityTechnologies/PaddleGameSO/blob/main/Assets/Core/EventChannels/ScriptableObjects/GenericEventChannelSO.cs */

[CreateAssetMenu(fileName = "New GameEvent", menuName = "Game Event/Void", order = 52)] // adds GameEvent as an asset in the asset menu
public class VoidGameEvent : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
    public UnityAction OnEventRaised;

    // ------------------------------- Functions -------------------------------
    [Button]
    public void RaiseEvent()
    {
        if (OnEventRaised == null) return;

        OnEventRaised.Invoke();
    }
}
