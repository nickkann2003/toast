using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* https://github.com/UnityTechnologies/PaddleGameSO/blob/main/Assets/Core/EventChannels/ScriptableObjects/GenericEventChannelSO.cs */

public class GenericGameEvent<T> : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
    public UnityAction<T> OnEventRaised;

    // ------------------------------- Functions -------------------------------
    public void RaiseEvent(T parameter)
    {
        if (OnEventRaised == null) return;

        OnEventRaised.Invoke(parameter);
    }

    // To create addition event channels, simply derive a class from GenericGameEvent
    // filling in the type T. Leave the concrete implementation blank. This is a quick way
    // to create new event channels.

    // For example:
    //   [CreateAssetMenu(fileName = "New Float GameEvent", menuName = "Game Event/Float", order = 52)]
    //   public class FloatGameEvent : GenericGameEvent<float> {}

    // Define additional GenericEventChannels if you need more than one parameter in the payload.
}

public class GenericGameEvent<T,S> : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [Tooltip("The action to perform; Listeners subscribe to this UnityAction")]
    public UnityAction<T,S> OnEventRaised;

    // ------------------------------- Functions -------------------------------
    public void RaiseEvent(T parameter1, S parameter2)
    {
        if (OnEventRaised == null) return;

        OnEventRaised.Invoke(parameter1, parameter2);
    }
}