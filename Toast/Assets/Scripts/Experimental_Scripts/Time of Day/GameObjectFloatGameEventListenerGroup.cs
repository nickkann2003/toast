using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFloatEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<GameObjectFloatGameEventListener> gameEventListeners;

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
public class GameObjectFloatGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(GameObject,Float) GameEvent"), AllowNesting]
    private GameObjectFloatGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField]
    private GameObjectFloatUnityEvent response; // the response that will be invoked

    public GameObjectFloatGameEvent GameEvent { get => gameEvent; set => gameEvent = value; }
    public GameObjectFloatUnityEvent Response { get => response; set => response = value; }

    public GameObjectFloatGameEventListener()
    {
        response = new GameObjectFloatUnityEvent();
    }

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

    public void OnEventRaised(GameObject value1, float value2) // when the event is raised, invoke the response
    {
        response.Invoke(value1, value2);
    }
}
