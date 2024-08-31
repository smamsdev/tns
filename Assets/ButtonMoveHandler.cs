using UnityEngine;
using UnityEngine.Events;

public class ButtonMoveHandler : MonoBehaviour
{
    [System.Serializable]

    public class ButtonMoveEvent : UnityEvent<int> { }

    public ButtonMoveEvent buttonClickEvent;

    public void ButtonClickWithInt(int moveValue)
    {
        if (CombatEvents.SendMove == null)
        {
            Debug.LogError("CombatEvents.SendMove is null!");
            return;
        }

        CombatEvents.SendMove.Invoke(moveValue);
    }
}