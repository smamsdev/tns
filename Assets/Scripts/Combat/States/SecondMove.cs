using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.secondMoveMenu, true);

        combatManager.combatMenuManager.secondMenuFirstButton = buttonSelected;
        combatManager.combatMenuManager.secondMenuFirstButton.Select();
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.secondMoveMenu, false);
            combatManager.SetState(combatManager.firstMove);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.buttonSelected, Color.white);
        }
    }

    public override void CombatOptionSelected(int moveValue) //triggered via Button
    {
        combatManager.playerCombat.playerMoveManager.secondMoveIs = moveValue;
        combatManager.combatMenuManager.secondMenuFirstButton = buttonSelected;
        combatManager.playerCombat.playerMoveManager.CombineStanceAndMove();

        if (combatManager.playerCombat.moveSelected.attackMoveModPercent > 0)
        {
            combatManager.SetState(combatManager.enemySelect);
        }
        else
        {
            combatManager.SetState(combatManager.allyMoveState);
        }
    }
}