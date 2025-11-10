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
            var enemy = combatManager.enemies[i];

            //reset narrator focus camera on allyToAct and wait
            combatManager.cameraFollow.transformToFollow = enemy.transform;
            yield return new WaitForSeconds(0.25f);

            var moveSelected = enemy.moveSelected;
            moveSelected.LoadMoveReferences(enemy, combatManager);
            CombatEvents.UpdateNarrator(moveSelected.moveSO.MoveName);

            yield return new WaitForSeconds(1f);
            moveSelected.CalculateMoveStats();
            CombatEvents.UpdateNarrator("");

            //rock out
            yield return moveSelected.ApplyMove(enemy, enemy.targetCombatant);
 
            i++;
        }

        combatManager.SetState(combatManager.roundReset);
    }
}