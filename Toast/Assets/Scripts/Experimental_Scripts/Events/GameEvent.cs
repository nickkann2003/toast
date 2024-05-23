using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

/* https://www.kodeco.com/2826197-scriptableobject-tutorial-getting-started/page/2 */

[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)] // adds GameEvent as an asset in the asset menu
public class GameEvent : ScriptableObject
{
    // ------------------------------- Variables -------------------------------
    [SerializeField, ReadOnly]
    private List<GameEventListener> listeners = new List<GameEventListener>(); // subscribers to the GameEvent

    // ------------------------------- Functions -------------------------------
    [Button] // inspector button
    public void RaiseEvent() // method to invoke all the subscribers
    {
        for (int i = listeners.Count - 1; i >= 0; i--) // last GameEventListener to subscribe will be the first to get invoked (last in, first out)
        {
            listeners[i].OnEventRaised(); // invoke the listeners
        }
    }

    public void RegisterListener(GameEventListener listener) // allows GameEventListeners to subscribe to the GameEvent
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener) // allows GameEventListeners to unsubscribe to the GameEvent
    {
        listeners.Remove(listener);
    }
}