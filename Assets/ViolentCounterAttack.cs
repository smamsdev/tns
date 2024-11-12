using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentCounterAttack : ViolentMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        enemy = _enemy;


        yield break;
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;
        enemy = _enemy;

        yield return new WaitForSeconds(1.0f);
        enemy.DamageTaken(combatManager.playerCombatStats.attackPower, combatManager.selectedPlayerMove.damageToPartsMultiplier);
        yield break;
    }

    public override IEnumerator Return()
    {
        Debug.Log("todo");
        yield return null;
    }
}
