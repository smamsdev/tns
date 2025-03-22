using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.roundCount++;

        combatManager.playerCombat.combatantUI.fendScript.animator.SetTrigger("fendFade");
        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;
        combatManager.playerCombat.attackTotal = 0;
        combatManager.playerCombat.fendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";
        combatManager.playerCombat.combatantUI.fendScript.ResetAllFendAnimationTriggers(); //its just easier this way 

        foreach (Enemy enemy in combatManager.enemies)
        {
            enemy.combatantUI.fendScript.ResetAllFendAnimationTriggers();
            combatManager.SelectAndDisplayCombatantMove(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            ally.combatantUI.fendScript.ResetAllFendAnimationTriggers();
            combatManager.SelectAndDisplayCombatantMove(ally);
        }

        combatManager.SetState(combatManager.firstMove);
        yield break;
    }

}
