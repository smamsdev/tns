using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMoveState : State
{
    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.secondMoveMenu, true);
        combatManager.combatMenuManager.styleMenuDefaultButton.Select();
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.secondMoveMenu, false);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.lastButtonSelected, Color.white);
            combatManager.firstMove.lastButtonSelected.Select();
            combatManager.SetState(combatManager.firstMove);
        }
    }

    public void StyleButtonSelected(int moveValue) //triggered via Button
    {
        combatManager.playerCombat.playerMoveManager.secondMoveIs = moveValue;
        combatManager.combatMenuManager.styleMenuDefaultButton = lastButtonSelected;
        combatManager.playerCombat.playerMoveManager.CombineStanceAndMove();

        if (!combatManager.playerCombat.moveSelected.moveSO.ApplyMoveToSelfOnly)
        {
            combatManager.SetState(combatManager.enemySelectState);
        }
        else
        {
            combatManager.playerCombat.targetCombatant = combatManager.playerCombat;
            combatManager.SetState(combatManager.applyMove);
        }
    }
}