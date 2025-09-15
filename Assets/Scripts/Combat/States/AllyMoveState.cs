using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
   public override IEnumerator StartState()
    {
        foreach (Ally ally in combatManager.allies)
        {
            ally.combatantUI.attackDisplay.ShowAttackDisplay(ally, false);
            ally.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
            ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
        }

        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat,false);

        for (int i = 0; i < combatManager.allies.Count;)
        {
            combatManager.lastCombatantTargeted = combatManager.allies[i].targetToAttack;

            //Disable untargeted combatant UI elements
            foreach (Enemy enemy in combatManager.enemies)
            {
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(enemy, false);

                if (enemy != combatManager.allies[i].targetToAttack)
                {
                    enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
                    enemy.combatantUI.statsDisplay.ShowStatsDisplay(false); 
                }
            }

            //store enemy target look dir
            var enemyTargetMovementScript = combatManager.allies[i].targetToAttack.GetComponent<MovementScript>();
            var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

            var allyToActMovementScript = combatManager.allies[i].GetComponent<MovementScript>();
            var allyToActLastLookDirection = allyToActMovementScript.lookDirection;

            var allyToActAnimator = combatManager.allies[i].gameObject.GetComponent<Animator>();
            allyToActAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on allyToAct and wait
            CombatEvents.UpdateNarrator("");
            combatManager.cameraFollow.transformToFollow = combatManager.allies[i].transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = combatManager.allies[i].targetToAttack.GetComponentInChildren<CombatantUI>();

            if (combatManager.allies[i].targetToAttack.fendTotal > 0)
            {
                targetToAttackUI.fendScript.ShowFendDisplay(combatManager.allies[i].targetToAttack, true);
                targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return combatManager.allies[i].moveSelected.ApplyMove(combatManager.allies[i], combatManager.allies[i].targetToAttack);
            combatManager.allies[i].GetComponent<MovementScript>().lookDirection = allyToActLastLookDirection;

            if (combatManager.allies[i].targetToAttack.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatManager.allies[i].targetToAttack);
            }

            //return target to original pos if still alive
            else
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(combatManager.allies[i].targetToAttack.gameObject, combatManager.allies[i].targetToAttack.fightingPosition.transform.position);
            }

            if (combatManager.allies[i].CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatManager.allies[i]);
            }

            else
            {
                i++;
            }
        }

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victory);
            yield break;
        }

        combatManager.SetState(combatManager.enemyMoveState);
    }
}
