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

        for (int i = 0; i < combatManager.enemies.Count;) //gotta manage an iterator here as the enemy list count may change mid loop
        {
            foreach (Combatant combatant in combatManager.allAlliesToTarget)
            {
                if (combatant != combatManager.enemies[i].targetToAttack)
                {
                    combatant.combatantUI.fendScript.ShowFendDisplay(combatant, false);
                    combatant.combatantUI.statsDisplay.ShowStatsDisplay(false);
                }
            }

            //store allied target look dir
            var alliedTargetMovementScript = combatManager.enemies[i].targetToAttack.GetComponent<MovementScript>();
            var alliedTargetStoredLookDir = alliedTargetMovementScript.lookDirection;

            var enemyLastLookDirection = combatManager.enemies[i].GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = combatManager.enemies[i].gameObject.GetComponent<Animator>();
            enemyAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on enemy to act and wait
            CombatEvents.UpdateNarrator.Invoke("");
            combatManager.cameraFollow.transformToFollow = combatManager.enemies[i].transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = combatManager.enemies[i].targetToAttack.GetComponentInChildren<CombatantUI>();

            if (!combatManager.enemies[i].targetToAttack.fendDisplayOn)
            {
                targetToAttackUI.fendScript.ShowFendDisplay(combatManager.enemies[i].targetToAttack, true);
                targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return combatManager.enemies[i].moveSelected.ApplyMove(combatManager.enemies[i], combatManager.enemies[i].targetToAttack);
            combatManager.enemies[i].GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //return target to original pos and look dir, if still alive
            if (combatManager.enemies[i].targetToAttack.CurrentHP != 0)
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(combatManager.enemies[i].targetToAttack.gameObject, combatManager.enemies[i].targetToAttack.fightingPosition.transform.position);
                alliedTargetMovementScript.lookDirection = alliedTargetStoredLookDir;
            }

            //check that enemy to act did not die mid turn and iterate
            if (combatManager.enemies[i].CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatManager.enemies[i]);
            }
            else
            {
                i++;
            }
        }
        combatManager.SetState(combatManager.roundReset);
    }
}