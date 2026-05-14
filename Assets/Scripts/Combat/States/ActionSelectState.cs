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
        combatManager.playerCombat.actionType = -1;

        yield break;
    }

    public void ActionButtonSelected(int moveValue) //triggered via Button
    {
        combatManager.playerCombat.actionType = moveValue;
        combatManager.playerCombat.CombineStanceAndMove();
        combatManager.SetState(combatManager.enemySelectState);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            actionSelectMenuUI.DisplayMenu(false);

            combatManager.SetState(combatManager.styleSelectState);
        }
    }
}