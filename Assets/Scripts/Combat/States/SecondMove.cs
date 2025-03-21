using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.secondMoveMenu);

        combatManager.combatMenuManager.secondMenuFirstButton.Select();
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

    public override void CombatOptionSelected(int moveValue) //triggered via Button
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

        if (combatManager.playerCombat.moveSelected.attackMoveModPercent > 0)
        {
            combatManager.SetState(combatManager.enemySelect);
        }
        else
        {
            //Disable other combatant UI elements
            foreach (Enemy enemy in combatManager.enemies)
            {
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(false);
                enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);

            }

            foreach (Ally ally in combatManager.allies)
            {
                ally.combatantUI.attackDisplay.ShowAttackDisplay(false);
                ally.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
            }

            combatManager.SetState(combatManager.allyMoveState);
        }
    }
}