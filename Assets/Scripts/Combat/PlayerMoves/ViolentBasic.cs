using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{

    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        enemy = _enemy;

        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(true);

        if (isAttack)
        {
            combatManager.UpdateFighterPosition(combatManager.player, new Vector2(combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.x - 0.3f, combatManager.battleScheme.enemyGameObject[combatManager.selectedEnemy].transform.position.y), 0.5f);
            yield return new WaitForSeconds(0.5f);
            combatManager.enemy[combatManager.selectedEnemy].enemyUI.enemyFendScript.ApplyPlayerAttackToFend(combatManager.playerCombatStats.attackPower);

            yield return new WaitForSeconds(0.3f);
            combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 0.5f);

            yield return new WaitForSeconds(0.5f);

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
