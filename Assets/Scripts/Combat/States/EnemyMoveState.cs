using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
    public override IEnumerator StartState()
    {
        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(false);
            enemy.combatantUI.statsDisplay.statsDisplayGameObject.SetActive(false);
            enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
        }

        foreach (Enemy enemyToAct in combatManager.enemies)
        {
            enemyToAct.targetToAttack = combatManager.allAlliesToTarget[Random.Range(0, combatManager.allAlliesToTarget.Count)];

            foreach (Combatant combatant in combatManager.allies)
            {
                if (combatant != enemyToAct.targetToAttack)
                {
                    combatant.combatantUI.fendScript.ShowFendDisplay(combatant, false);
                    combatant.combatantUI.statsDisplay.ShowStatsDisplay(false);
                }
            }

            //store allied target look dir
            var alliedTargetMovementScript = enemyToAct.targetToAttack.GetComponent<MovementScript>();
            var alliedTargetStoredLookDir = alliedTargetMovementScript.lookDirection;

            var enemyLastLookDirection = enemyToAct.GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = enemyToAct.gameObject.GetComponent<Animator>();
            enemyAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on enemy to act and wait
            CombatEvents.UpdateNarrator.Invoke("");
            combatManager.cameraFollow.transformToFollow = enemyToAct.transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = enemyToAct.targetToAttack.GetComponentInChildren<CombatantUI>();

            if (!enemyToAct.targetToAttack.fendDisplayOn)
            {
                targetToAttackUI.fendScript.ShowFendDisplay(enemyToAct.targetToAttack, true);
                targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return enemyToAct.moveSelected.ApplyMove(enemyToAct, enemyToAct.targetToAttack);
            enemyToAct.GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //return target to original pos and look dir, if still alive
            if (enemyToAct.targetToAttack.CurrentHP != 0)
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(enemyToAct.targetToAttack.gameObject, enemyToAct.targetToAttack.fightingPosition.transform.position);
                alliedTargetMovementScript.lookDirection = alliedTargetStoredLookDir;
            }
        }
        combatManager.SetState(combatManager.roundReset);
    }
}