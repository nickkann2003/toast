using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatGameEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<FloatGameEventListener> gameEventListeners;

    private void OnEnable() // when enabled, enable all of the listeners
    {
        if (gameEventListeners.Count == 0) return;

        for (int i = 0; i < gameEventListeners.Count; i++)
        {
            gameEventListeners[i].OnEnable();
        }
    }

    private void OnDisable() // when disabled, disable all of the listeners
    {
        if (gameEventListeners.Count == 0) return;

        for (int i = 0; i < gameEventListeners.Count; i++)
        {
            gameEventListeners[i].OnDisable();
        }
    }
}

[Serializable]
public class FloatGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(Float) GameEvent"), AllowNesting]
    private FloatGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField]
    private FloatUnityEvent response; // the response that will be invoked

    public void OnEnable() // when enabled, subscribe to the GameEvent
    {
        if (gameEvent == null || enabled) return;

        gameEvent.OnEventRaised += OnEventRaised;
        enabled = true;
    }

    public void OnDisable() // when disabled, unsubscribe from the GameEvent
    {
        if (gameEvent == null || !enabled) return;

        gameEvent.OnEventRaised -= OnEventRaised;
        enabled = false;
    }

    public void OnEventRaised(float value) // when the event is raised, invoke the response
    {
        response.Invoke(value);
    }
}