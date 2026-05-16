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
        actionSelectMenuUI.DisplayMenu(true);
        combatManager.playerCombat.actionType = -1;

        yield break;
    }

    public void ActionButtonSelected(int moveValue)
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
            combatManager.combatMenuManager.UpdateNarrator("");
            combatManager.SetState(combatManager.styleSelectState);
        }
    }
}