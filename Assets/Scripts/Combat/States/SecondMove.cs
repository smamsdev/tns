using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.secondMoveMenu);

        combatManager.combatMenuManager.secondMenuFirstButton.Select();
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.SetState(combatManager.firstMove);
        }
    }

    public override void CombatOptionSelected(int moveValue) //triggered via Button
    {
        combatManager.playerCombat.playerMoveManager.secondMoveIs = moveValue;
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