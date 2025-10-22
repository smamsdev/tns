using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : ViolentMove
{
    public override IEnumerator OnReceieveAttack(Combatant combatantApplying, Combatant combatantReceiving)
    {
        var combatantApplyingMovmentScript = combatantApplying.GetComponent<MovementScript>();
        var combatantReceivingMovmentScript = combatantReceiving.GetComponent<MovementScript>();

        if (combatantApplyingMovmentScript.lookDirection != combatantReceivingMovmentScript.lookDirection)
        {
            GetReferences(combatantReceiving, combatantApplying);
            attackMoveModPercent = 2;
            combatManager.playerCombat.TotalPlayerAttackPower(attackMoveModPercent);

            combatantApplying.combatantUI.fendScript.ApplyAttackToFend(combatantToAct, combatantApplying);
            TriggerMoveAnimation();



            yield return new WaitForSeconds(1f);


            attackMoveModPercent = 0;
            CombatEvents.TriggerAnimationOnce(combatantToActAnimator, "CombatIdle");
        }

        yield return null;
    }

    public override void GetReferences(Combatant combatantToAct, Combatant targetCombatant)
    {
        combatantToActAnimator = combatantToAct.GetComponent<Animator>();
        combatantToActMovementScript = combatantToAct.GetComponent<MovementScript>();
        this.combatantToAct = combatantToAct;
        this.targetCombatant = targetCombatant;
    }
}
