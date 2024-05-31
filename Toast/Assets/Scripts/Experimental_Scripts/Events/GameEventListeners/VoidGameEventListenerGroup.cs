using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* https://github.com/UnityTechnologies/PaddleGameSO/blob/main/Assets/Core/EventChannels/Scripts/VoidEventChannelListener.cs */

public class VoidGameEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<VoidGameEventListener> gameEventListeners;

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
public class VoidGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(Void) GameEvent"), AllowNesting]
    private VoidGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField] 
    private UnityEvent response; // the response that will be invoked

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

    public void OnEventRaised() // when the event is raised, invoke the response
    {
        response.Invoke();
    }
}