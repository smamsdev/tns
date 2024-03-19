using UnityEngine;
using UnityEngine.Events;

public class ButtonStateHandler : MonoBehaviour
{
    [System.Serializable]
    public class ButtonStateEvent : UnityEvent<State, int> { }

    public ButtonStateEvent buttonClickEvent;

    public void ButtonClick(State state)
    {
        CombatEvents.SendState.Invoke(state);
    }
}