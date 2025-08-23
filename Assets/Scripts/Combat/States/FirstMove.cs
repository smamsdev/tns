using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstMove : State
{
    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(true);
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.firstMoveMenu, true);
        combatManager.combatMenuManager.firstMenuFirstButton = buttonSelected;
        combatManager.combatMenuManager.firstMenuFirstButton.Select();
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;

        yield break;
    }

    public override void CombatOptionSelected (int moveValue)
    {
        combatManager.combatMenuManager.firstMenuFirstButton = buttonSelected;
        combatManager.playerCombat.playerMoveManager.firstMoveIs = moveValue;

        if (moveValue == 4)
        {
            combatManager.SetState(combatManager.gearSelectState);
        }
        else
        {
            combatManager.SetState(combatManager.secondMove);
        }
    }
}