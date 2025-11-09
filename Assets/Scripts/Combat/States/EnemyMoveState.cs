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

        for (int i = 0; i < combatManager.enemies.Count;) //manage an iterator not a foreach as the enemy list count may change mid loop
        {
            yield return ApplyEnemyMove(combatManager.enemies[i]);

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
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

    IEnumerator ApplyEnemyMove(Combatant enemyIteration)
    { 
        combatManager.cameraFollow.transformToFollow = enemyIteration.transform;
        CombatEvents.UpdateNarrator.Invoke(enemyIteration.moveSelected.moveSO.MoveName);
        yield return new WaitForSeconds(0.5f);
        yield return enemyIteration.moveSelected.ApplyMove(enemyIteration, enemyIteration.targetCombatant);
    }
}