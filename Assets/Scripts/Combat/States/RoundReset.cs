using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.combatUIScript.playerFendScript.animator.SetTrigger("fendFade");

        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetAttackGear();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetFendGear();

        combatManager.attackTargetMenuScript.attackTargetMenu.SetActive(false);
        combatManager.attackTargetMenuScript.targetSelected = false;
        combatManager.attackTargetMenuScript.targetIsSet = 0;

        combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ShowFendDisplay(false);

        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;

        yield return new WaitForSeconds(0.5f);

        combatManager.roundCount++;

        foreach (Enemy enemy in combatManager.enemy)
        {
            enemy.SelectEnemyMove();

            if (enemy.attackTotal > 0)
            {
                enemy.enemyUI.enemyAttackDisplay.UpdateEnemyAttackDisplay(combatManager.enemy[combatManager.selectedEnemy].EnemyAttackTotal());
                enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(true);
            }

            if (enemy.fendTotal > 0)
            {
                enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
                enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(true);
            }
        }

        combatManager.playerCombatStats.attackPower = 0;
        combatManager.playerCombatStats.playerFend = 0;

        combatManager.combatUIScript.playerFendScript.UpdateFendText(0);
        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}
