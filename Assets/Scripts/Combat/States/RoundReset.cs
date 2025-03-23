using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.roundCount++;

        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;
        combatManager.playerCombat.attackTotal = 0;
        combatManager.playerCombat.fendTotal = 0;
        combatManager.playerCombat.combatantUI.fendScript.fendTextMeshProUGUI.text = "0";

        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectAndDisplayCombatantMove(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectAndDisplayCombatantMove(ally);
        }

        combatManager.SetState(combatManager.firstMove);
        yield break;
    }

}
