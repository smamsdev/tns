using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseBotch : PreciseMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)
    {
        combatManager = _combatManager;

        yield return new WaitForSeconds(1.0f);
        combatManager.applyMove.EndMove();
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }
}
