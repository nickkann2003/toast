using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PropIntEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<PropIntGameEventListener> gameEventListeners;

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
public class PropIntGameEventListener
{
    //[SerializeField, ReadOnly, AllowNesting]
    private bool enabled;
    [SerializeField, Label("(Prop,Int) GameEvent"), AllowNesting]
    private PropIntGameEvent gameEvent; // the GameEvent this listener will subscribe to
    [SerializeField]
    private PropIntUnityEvent response; // the response that will be invoked

    public PropIntGameEvent GameEvent { get => gameEvent; set => gameEvent = value; }
    public PropIntUnityEvent Response { get => response; set => response = value; }

    public PropIntGameEventListener()
    {
        response = new PropIntUnityEvent();
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

    public void OnEventRaised(NewProp value1, int value2) // when the event is raised, invoke the response
    {
        response.Invoke(value1, value2);
    }
}