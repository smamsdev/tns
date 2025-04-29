using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.attackTargetMenu);
        combatManager.combatMenuManager.targetMenuFirstButton.Select();

        var targetUI = combatManager.playerCombat.targetToAttack.combatantUI as EnemyUI;
        targetUI.partsTargetDisplay.UpdateTargetDisplay(true, false, false);

        yield break;
    }

    public override void CombatOptionSelected(int moveValue) //triggered via Button
    {
        var enemyToAttack = combatManager.playerCombat.targetToAttack as Enemy;
        enemyToAttack.SetEnemyBodyPartTarget(moveValue);
        DisablePartsTargetDisplay();
        combatManager.combatMenuManager.DisableMenuState();

        if (combatManager.allies.Count > 0)
        {
            combatManager.SetState(combatManager.allyMoveState);
        }

        else
        {
            combatManager.SetState(combatManager.applyMove); 
        }
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            DisablePartsTargetDisplay();

            if (combatManager.battleScheme.enemies.Count == 1)

            {
                combatManager.SetState(combatManager.secondMove);
                combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
            }

            else
            {
                combatManager.SetState(combatManager.enemySelect);
            }
        }
    }

    void DisablePartsTargetDisplay()
    {
        var targetUI = combatManager.playerCombat.targetToAttack.combatantUI as EnemyUI;
        targetUI.partsTargetDisplay.UpdateTargetDisplay(false, false, false);
    }
}

