using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
    public override IEnumerator StartState()
    {
        if (combatManager.battleScheme.isEnemyFlanked)
        {
            combatManager.battleScheme.isEnemyFlanked = false;
            combatManager.SetState(combatManager.roundReset);
            yield break;
        }

        for (int i = 0; i < combatManager.enemies.Count;) //gotta manage an iterator here as the enemy list count may change mid loop
        {
            //reset narrator focus camera on enemy to act and wait
            CombatEvents.UpdateNarrator.Invoke("");
            combatManager.cameraFollow.transformToFollow = combatManager.enemies[i].transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = combatManager.enemies[i].targetToAttack.GetComponentInChildren<CombatantUI>();

            if (combatManager.enemies[i].targetToAttack.fendTotal > 0)
            {
                targetToAttackUI.fendScript.ShowFendDisplay(combatManager.enemies[i].targetToAttack, true);
                targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return combatManager.enemies[i].moveSOSelected.moveInstance.ApplyMove(combatManager.enemies[i], combatManager.enemies[i].targetToAttack);

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //return target to original pos if still alive
            if (combatManager.enemies[i].targetToAttack.CurrentHP != 0)
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(combatManager.enemies[i].targetToAttack.gameObject, combatManager.enemies[i].targetToAttack.fightingPosition.transform.position);
                targetToAttackUI.fendScript.ShowFendDisplay(combatManager.enemies[i].targetToAttack, false);
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