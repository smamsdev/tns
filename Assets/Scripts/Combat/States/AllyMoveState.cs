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

            var moveSelected = ally.moveSelected;
            moveSelected.LoadMoveReferences(ally, combatManager);
            CombatEvents.UpdateNarrator(moveSelected.moveSO.MoveName);

            yield return new WaitForSeconds(1f);
            moveSelected.CalculateMoveStats();
            CombatEvents.UpdateNarrator("");

            //rock out
            yield return moveSelected.ApplyMove(ally, ally.targetCombatant);
  
            i++;
        }

        if (combatManager.enemies.Count == 0)
        {
            combatManager.SetState(combatManager.victory);
            yield break;
        }

        combatManager.SetState(combatManager.enemyMoveState);
    }
}
