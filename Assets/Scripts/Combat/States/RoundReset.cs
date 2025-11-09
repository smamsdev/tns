using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat, false);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.actionMenuDefaultButton, Color.white);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.styleMenuDefaultButton, Color.white);


        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
            enemy.combatantUI.attackDisplay.SetAttackDisplayDirBasedOnLookDir(enemy);

            combatManager.SelectCombatantMove(enemy);
            enemy.combatantUI.DisplayCombatantMove(enemy);

            yield return new WaitForSeconds(1f);
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(enemy, false);
            enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectTargetToAttack(ally, combatManager.enemies);
            ally.combatantUI.attackDisplay.SetAttackDisplayDirBasedOnLookDir(ally);

            combatManager.SelectCombatantMove(ally);
            ally.combatantUI.DisplayCombatantMove(ally);
            yield return new WaitForSeconds(1f);
            ally.combatantUI.attackDisplay.ShowAttackDisplay(ally, false);
            ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
        }

        yield return new WaitForSeconds(0.5f);

        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
        combatManager.roundCount++;
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;
        combatManager.playerCombat.AttackTotal = 0;
        combatManager.playerCombat.FendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";
        combatManager.SetState(combatManager.firstMove);
        yield break;
    }
}
