using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMoveState : State
{
    public override IEnumerator StartState()
    {
        if (combatManager.battleScheme.isAllyFlanked)
        {
            combatManager.battleScheme.isAllyFlanked = false;
            combatManager.SetState(combatManager.enemyMoveState);
            yield break;
        }

        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(true);
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.firstMoveMenu, true);
        combatManager.combatMenuManager.actionMenuDefaultButton.Select();
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;

        yield break;
    }

    public void ActionButtonSelected (int moveValue)
    {
        combatManager.combatMenuManager.actionMenuDefaultButton = lastButtonSelected;
        combatManager.playerCombat.playerMoveManager.firstMoveIs = moveValue;

        if (moveValue == 0)
        {
            combatManager.SetState(combatManager.tacticalSelectState);
        }
        else
        {
            combatManager.SetState(combatManager.secondMove);
        }
    }
}