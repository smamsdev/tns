using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.ShowFendDisplay(combatManager.playerCombat, false);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.firstMenuFirstButton, Color.white);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.secondMenuFirstButton, Color.white);
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.thirdMenuFirstButton, Color.white);

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
            combatManager.SelectAndDisplayCombatantMove(enemy);
            enemy.combatantUI.statsDisplay.ShowStatsDisplay(true);
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectTargetToAttack(ally, combatManager.enemies);
            ally.combatantUI.statsDisplay.ShowStatsDisplay(true);
            combatManager.SelectAndDisplayCombatantMove(ally);
        }

        yield return new WaitForSeconds(0.5f);


        combatManager.roundCount++;
        combatManager.playerCombat.playerMoveManager.firstMoveIs = 0;
        combatManager.playerCombat.playerMoveManager.secondMoveIs = 0;
        combatManager.playerCombat.attackTotal = 0;
        combatManager.playerCombat.fendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";
        combatManager.SetState(combatManager.firstMove);
        yield break;
    }
}
