using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;

    public override IEnumerator StartState()
    {
        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(false);

        combatManager.combatUIScript.HideSecondMenu();
        combatManager.combatUIScript.HideTargetMenu();

       // combatManager.playerCombatStats.ResetAllMoveMods();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetAttackGear();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetFendGear();

        combatManager.attackTargetMenuScript.attackTargetMenu.SetActive(false);
        combatManager.attackTargetMenuScript.targetSelected = false;
        combatManager.attackTargetMenuScript.targetIsSet = 0;

        combatManager.combatUIScript.playerFendScript.UpdateFendText(0);
        combatManager.combatUIScript.playerFendScript.FendIconAnimationState(2);
        combatManager.combatUIScript.enemyFendScript.FendIconAnimationState(0);
        combatManager.combatUIScript.enemyFendScript.ShowHideFendDisplay(false);

        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;

        combatManager.roundCount++;

        combatManager.enemy.SelectEnemyMove();
        CombatEvents.UpdateEnemyFendDisplay?.Invoke(combatManager.enemy.fendTotal);

        if (combatManager.enemy.attackTotal > 0)
        {
            CombatEvents.UpdateEnemyAttackDisplay?.Invoke(combatManager.enemy.EnemyAttackTotal());
            enemyAttackDisplay.SetActive(true);
        }

        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}
