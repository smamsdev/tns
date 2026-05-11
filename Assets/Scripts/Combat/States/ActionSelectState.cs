using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSelectState : State
{
    public ActionSelectMenuUI actionSelectMenuUI;

    public override IEnumerator StartState()
    {
        if (combatManager.battleScheme.isAllyFlanked)
        {
            combatManager.battleScheme.isAllyFlanked = false;
            combatManager.SetState(combatManager.enemyMoveState);
            yield break;
        }

        actionSelectMenuUI.SetButtonNormalColor(actionSelectMenuUI.menuButtons[actionSelectMenuUI.highlightedButtonIndex], Color.white);
        actionSelectMenuUI.menuButtons[actionSelectMenuUI.highlightedButtonIndex].Select();
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(true);
        actionSelectMenuUI.DisplayMenu(true);
        combatManager.playerCombat.playerMoveManager.actionSelectStateIs = 0;

        yield break;
    }

    public void ActionButtonSelected (int moveValue)
    {
        combatManager.playerCombat.playerMoveManager.actionSelectStateIs = moveValue;

        if (moveValue == 0)
        {
            combatManager.SetState(combatManager.tacticalSelectState);
        }
        else
        {
            combatManager.SetState(combatManager.styleSelectState);
        }
    }
}