using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BoolGameEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<BoolGameEventListener> gameEventListeners;

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
public class BoolGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(Bool) GameEvent"), AllowNesting]
    private BoolGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField]
    private BoolUnityEvent response; // the response that will be invoked

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

    public void OnEventRaised(bool value) // when the event is raised, invoke the response
    {
        response.Invoke(value);
    }
}