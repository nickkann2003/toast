using UnityEngine;
using UnityEngine.Events; // 1

/* https://www.kodeco.com/2826197-scriptableobject-tutorial-getting-started/page/2 */

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent; // the GameEvent this GameEventListener will subscribe to
    [SerializeField]
    private UnityEvent response; // the response that will be invoked

    private void OnEnable() // when enabled, subscribe to the GameEvent
    {
        if (gameEvent == null)
        {
            Debug.Log("Game Event is not set");
            return;
        }
        gameEvent.RegisterListener(this);
    }

    private void OnDisable() // when disabled, unsubscribe from the GameEvent
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