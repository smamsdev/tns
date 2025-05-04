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
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.combatMenuManager.targetMenuFirstButton, Color.white);

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.targetToAttack = combatManager.allAllies[Random.Range(0, combatManager.allAllies.Count)];
            combatManager.SelectAndDisplayCombatantMove(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            ally.targetToAttack = combatManager.enemies[Random.Range(0, combatManager.enemies.Count)];
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
