using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{

    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;

        var enemyPosition = combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position;


        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(true);

        if (isAttack)
        {
            yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, enemyPosition);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower);
            yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);
            combatManager.applyMove.EndMove();
        }

        if (!isAttack)

        {
            yield return new WaitForSeconds(0.5f);
            combatManager.applyMove.EndMove();
        }

    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }
}
