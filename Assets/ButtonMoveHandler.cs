using UnityEngine;
using UnityEngine.Events;

public class ButtonMoveHandler : MonoBehaviour
{
    [System.Serializable]
    public class ButtonMoveEvent : UnityEvent<int> { }

    public ButtonMoveEvent buttonClickEvent;

    public void ButtonClickWithInt(int moveValue)
    {
        CombatEvents.SendMove.Invoke(moveValue);
    }
}