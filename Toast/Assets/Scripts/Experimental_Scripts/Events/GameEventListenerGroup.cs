using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // 1

/* https://www.kodeco.com/2826197-scriptableobject-tutorial-getting-started/page/2 */

public class GameEventListenerGroup : MonoBehaviour
{
    [SerializeField]
    List<GameEventListener> gameEventListeners;

    private void OnEnable()
    {
        if (gameEventListeners.Count == 0) return;

        for (int i = 0; i < gameEventListeners.Count; i++)
        {
            gameEventListeners[i].OnEnable();
        }
    }

    private void OnDisable()
    {
        if (gameEventListeners.Count == 0) return;

        for (int i = 0; i < gameEventListeners.Count; i++)
        {
            gameEventListeners[i].OnDisable();
        }
    }
}

[Serializable]
public class GameEventListener
{
    [SerializeField]
    private GameEvent gameEvent; // the GameEvent this GameEventListener will subscribe to
    [SerializeField]
    private UnityEvent response; // the response that will be invoked

    public void OnEnable() // when enabled, subscribe to the GameEvent
    {
        if (gameEvent == null)
        {
            Debug.Log("Game Event is not set");
            return;
        }
        gameEvent.RegisterListener(this);
    }

    public void OnDisable() // when disabled, unsubscribe from the GameEvent
    {
        if (gameEvent == null)
        {
            Debug.Log("Game Event is not set");
            return;
        }
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised() // when the event is invoked, invoke the response
    {
        response.Invoke();
    }
}