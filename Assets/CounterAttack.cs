using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : Move
{
    public override IEnumerator OnReceieveAttack(Combatant combatantApplying, Combatant combatantReceiving)
    {
        var combatantApplyingMovmentScript = combatantApplying.GetComponent<MovementScript>();
        var combatantReceivingMovmentScript = combatantReceiving.GetComponent<MovementScript>();

        // if (combatantApplyingMovmentScript.lookDirection != combatantReceivingMovmentScript.lookDirection)
        // {
        //     GetReferences(combatantReceiving, combatantApplying);
        //     moveSO.attackMoveModPercent = 2;
        //     combatManager.playerCombat.TotalPlayerAttackPower(moveSO.attackMoveModPercent);
        //
        //     combatantApplying.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatantApplying);
        //     yield return TriggerMoveAnimation();
        //
        //
        //
        //     yield return new WaitForSeconds(1f);
        //
        //
        //     moveSO.attackMoveModPercent = 0;
        //     combatantToActAnimator.SetTrigger("CombatIdle");
        // }

        Debug.Log("rework");

        yield return null;
    }
}
