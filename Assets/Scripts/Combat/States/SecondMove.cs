using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.CombatUIManager.ChangeMenuState(combatManager.CombatUIManager.secondMoveMenu);

        combatManager.CombatUIManager.secondMenuFirstButton.Select();
        combatManager.CombatUIManager.UpdateSecondMoveDisplay("Move?");
        combatManager.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.SetState(combatManager.firstMove);
        }
    }

    public override void CombatOptionSelected(int moveValue)
    {
        combatManager.playerMoveManager.secondMoveIs = moveValue;
        string moveName = "";

        switch (moveValue)
        {
            case 1:
                moveName = "Attack";
                break;
            case 2:
                moveName = "Fend";
                break;
            case 3:
                moveName = "Focus";
                break;
        }

        combatManager.playerMoveManager.CombineStanceAndMove();
        combatManager.selectedPlayerMove = combatManager.playerMoveManager.GetSelectedPlayerMove();

        if (combatManager.selectedPlayerMove.attackMoveModPercent > 0)
        {
            combatManager.SetState(combatManager.enemySelect);
        }
        else
        {
            combatManager.SetState(combatManager.applyMove);
        }

        combatManager.CombatUIManager.UpdateSecondMoveDisplay(moveName);

    }

    private void OnDisable()
    {
        combatManager = null;
    }
}