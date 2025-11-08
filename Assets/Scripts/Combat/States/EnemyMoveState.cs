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

            var targetCombatantUI = combatManager.enemies[i].targetCombatant.GetComponentInChildren<CombatantUI>();

            if (combatManager.enemies[i].targetCombatant.fendTotal > 0)
            {
                targetCombatantUI.fendScript.ShowFendDisplay(combatManager.enemies[i].targetCombatant, true);
                targetCombatantUI.statsDisplay.ShowStatsDisplay(true);
            }

            yield return combatManager.enemies[i].moveSelected.ApplyMove(combatManager.enemies[i], combatManager.enemies[i].targetCombatant);

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //return target to original pos if still alive
            if (combatManager.enemies[i].targetCombatant.CurrentHP != 0)
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(combatManager.enemies[i].targetCombatant.gameObject, combatManager.enemies[i].targetCombatant.fightingPosition.transform.position);
                targetCombatantUI.fendScript.ShowFendDisplay(combatManager.enemies[i].targetCombatant, false);
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