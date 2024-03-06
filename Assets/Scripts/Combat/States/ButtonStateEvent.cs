using UnityEngine;
using UnityEngine.Events;

public class ButtonStateEvent : MonoBehaviour
{

    [System.Serializable]
    public class ButtonClickEvent : UnityEvent<State> { }

    public ButtonClickEvent buttonClickEvent;

    public void ButtonClick(State state)
    {
        CombatEvents.PassState.Invoke(state);
    }
}