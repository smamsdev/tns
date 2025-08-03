using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
   public override IEnumerator StartState()
    {
        foreach (Ally ally in combatManager.allies)
        {
            ally.combatantUI.attackDisplay.ShowAttackDisplay(false);
            ally.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
            ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
        }

        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat,false);

        foreach (Ally allyToAct in combatManager.allies)
        {
            combatManager.lastCombatantTargeted = allyToAct.targetToAttack;

            //Disable untargeted combatant UI elements
            foreach (Enemy enemy in combatManager.enemies)
            {
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(false);

                if (enemy != allyToAct.targetToAttack)
                {
                    enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
                    enemy.combatantUI.statsDisplay.ShowStatsDisplay(false); 
                }
            }

            //store enemy target look dir
            var enemyTargetMovementScript = allyToAct.targetToAttack.GetComponent<MovementScript>();
            var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

            var allyToActMovementScript = allyToAct.GetComponent<MovementScript>();
            var allyToActLastLookDirection = allyToActMovementScript.lookDirection;

            var allyToActAnimator = allyToAct.gameObject.GetComponent<Animator>();
            allyToActAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on allyToAct and wait
            CombatEvents.UpdateNarrator("");
            combatManager.cameraFollow.transformToFollow = allyToAct.transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = allyToAct.targetToAttack.GetComponentInChildren<CombatantUI>();

            if (!allyToAct.targetToAttack.fendDisplayOn)
            {
                targetToAttackUI.fendScript.ShowFendDisplay(allyToAct.targetToAttack, true);
                targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return allyToAct.moveSelected.ApplyMove(allyToAct, allyToAct.targetToAttack);
            allyToAct.GetComponent<MovementScript>().lookDirection = allyToActLastLookDirection;

            if (allyToAct.targetToAttack.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(allyToAct.targetToAttack);
            }

            //return target to original pos and look dir, if still alive
            else
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(allyToAct.targetToAttack.gameObject, allyToAct.targetToAttack.fightingPosition.transform.position);
            }
        }
        combatManager.SetState(combatManager.enemyMoveState);
    }
}
