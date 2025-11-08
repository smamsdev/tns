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
            combatManager.lastCombatantTargeted = combatManager.allies[i].targetCombatant;

            //Disable untargeted combatant UI elements
            foreach (Enemy enemy in combatManager.enemies)
            {
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(enemy, false);

                if (enemy != combatManager.allies[i].targetCombatant)
                {
                    enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
                    enemy.combatantUI.statsDisplay.ShowStatsDisplay(false); 
                }
            }

            //store enemy target look dir
            var enemyTargetMovementScript = combatManager.allies[i].targetCombatant.GetComponent<MovementScript>();
            var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

            var allyToActMovementScript = combatManager.allies[i].GetComponent<MovementScript>();
            var allyToActLastLookDirection = allyToActMovementScript.lookDirection;

            var allyToActAnimator = combatManager.allies[i].gameObject.GetComponent<Animator>();
            allyToActAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on allyToAct and wait
            CombatEvents.UpdateNarrator("");
            combatManager.cameraFollow.transformToFollow = combatManager.allies[i].transform;
            yield return new WaitForSeconds(0.5f);

            var targetCombatantUI = combatManager.allies[i].targetCombatant.GetComponentInChildren<CombatantUI>();

            if (combatManager.allies[i].targetCombatant.fendTotal > 0)
            {
                targetCombatantUI.fendScript.ShowFendDisplay(combatManager.allies[i].targetCombatant, true);
                targetCombatantUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return combatManager.allies[i].moveSelected.ApplyMove(combatManager.allies[i], combatManager.allies[i].targetCombatant);
            combatManager.allies[i].GetComponent<MovementScript>().lookDirection = allyToActLastLookDirection;

            if (combatManager.allies[i].targetCombatant.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatManager.allies[i].targetCombatant);
            }

            //return target to original pos if still alive
            else
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(combatManager.allies[i].targetCombatant.gameObject, combatManager.allies[i].targetCombatant.fightingPosition.transform.position);
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
