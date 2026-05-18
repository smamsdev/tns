using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat, false);

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.SelectMove(combatManager);
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
            enemy.currentMoveBehaviour.LoadMoveReferences(enemy, combatManager);
            enemy.currentMoveBehaviour.CalculateMoveStats();
            enemy.combatantUI.attackDisplay.SetAttackDisplayDirBasedOnLookDir(enemy);
            enemy.combatantUI.DisplayCombatantMove(enemy);

            yield return new WaitForSeconds(1f);
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(enemy, false);
            enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
        }

        foreach (Ally ally in combatManager.allies)
        {
            ally.SelectMove(combatManager);
            combatManager.SelectTargetToAttack(ally, combatManager.allAlliesToTarget);
            ally.currentMoveBehaviour.LoadMoveReferences(ally, combatManager);
            ally.currentMoveBehaviour.CalculateMoveStats();
            ally.combatantUI.attackDisplay.SetAttackDisplayDirBasedOnLookDir(ally);
            ally.combatantUI.DisplayCombatantMove(ally);

            yield return new WaitForSeconds(1f);
            ally.combatantUI.attackDisplay.ShowAttackDisplay(ally, false);
            ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
        }

        yield return new WaitForSeconds(0.5f);

        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
        combatManager.roundCount++;
        combatManager.playerCombat.actionType = -1;
        combatManager.playerCombat.styleType = -1;
        combatManager.playerCombat.AttackTotal = 0;
        combatManager.playerCombat.FendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";
        combatManager.SetState(combatManager.styleSelectState);
        yield break;
    }
}
