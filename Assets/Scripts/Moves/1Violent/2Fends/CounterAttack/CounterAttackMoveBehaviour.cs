using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : MoveBehaviour
{
    public override IEnumerator OnReceieveAttack(Combatant combatantApplying, Combatant combatantReceiving)
    {
        if (combatantApplying.CombatLookDirX != combatantReceiving.CombatLookDirX)
        {
            combatantReceiving.combatantUI.fendScript.ShowFendDisplay(combatantReceiving, false);
            combatantReceiving.AttackTotal = combatantReceiving.AttackBase * 2;
            combatantReceiving.targetCombatant = combatantApplying;

            yield return ApplyMoveToEnemy();
           // combatantReceiving.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatantApplying);
           // yield return TriggerMoveAnimation();
           // yield return new WaitForSeconds(1f);
           //
           // combatantReceiving..SetTrigger("CombatIdle");
        }

        yield return null;
    }
}
