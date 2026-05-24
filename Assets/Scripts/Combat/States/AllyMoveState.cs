using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
   public override IEnumerator StartState()
    {
        if (combatManager.battleScheme.isAllyFlanked)
        {
            combatManager.SetState(combatManager.enemyMoveState);
            yield break;
        }

        for (int i = 0; i < combatManager.allies.Count;)
        {
            var ally = combatManager.allies[i];

            //reset narrator focus camera on allyToAct
            combatManager.cameraFollow.transformToFollow = ally.transform;
            yield return new WaitForSeconds(0.25f);

            combatManager.SelectTargetToAttack(ally, combatManager.enemies);
            ally.SelectMove(combatManager);
            ally.currentMoveBehaviour.LoadMoveReferences(ally, combatManager);
            ally.currentMoveBehaviour.CalculateMoveStats();
            //SetcombatantUI(ally);
            //ally.combatantUI.DisplayCombatantMove(ally);
            //yield return new WaitForSeconds(1f);
            //ally.combatantUI.attackDisplay.ShowAttackDisplay(ally, false);
            //ally.combatantUI.fendScript.ShowFendDisplay(ally, false);

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
