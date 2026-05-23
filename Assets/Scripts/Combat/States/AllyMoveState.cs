using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
   public override IEnumerator StartState()
    {
        for (int i = 0; i < combatManager.allies.Count;)
        {
            var ally = combatManager.allies[i];

            //reset narrator focus camera on allyToAct
            combatManager.cameraFollow.transformToFollow = ally.transform;
            yield return new WaitForSeconds(0.25f);

            var currentMoveBehaviour = ally.currentMoveBehaviour;
            combatManager.combatMenuManager.UpdateNarrator(currentMoveBehaviour.moveSO.MoveName);

            yield return new WaitForSeconds(1f);
            combatManager.combatMenuManager.UpdateNarrator("");

            //rock out
            yield return currentMoveBehaviour.ApplyMove(ally, ally.targetCombatant);
  
            i++;
        }

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victoryState);
            yield break;
        }

        combatManager.SetState(combatManager.enemyMoveState);
    }
}
