using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject firstMoveContainer;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(true);
        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.firstMoveMenu);
        combatManager.combatMenuManager.firstMenuFirstButton.Select();
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;

        yield break;
    }

    public override void CombatOptionSelected (int moveValue)
    {
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