using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGameEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<GameObjectGameEventListener> gameEventListeners;

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
public class GameObjectGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(GameObject) GameEvent"), AllowNesting]
    private GameObjectGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField]
    private GameObjectUnityEvent response; // the response that will be invoked

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

    public void OnEventRaised(GameObject value) // when the event is raised, invoke the response
    {
        response.Invoke(value);
    }
}