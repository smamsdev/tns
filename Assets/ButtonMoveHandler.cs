using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonMoveHandler : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;

    public void ButtonClickWithInt(int moveValue)
    {
        combatManager.currentState.CombatOptionSelected(moveValue);
    }
}