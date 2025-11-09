using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecklessFocus : Move
{
    public override void CalculateMoveStats()
    {
        combatantToAct.AttackTotal = (combatantToAct.MaxHP / 4);

        //leave at least 1 HP
        if (combatantToAct.AttackTotal >= combatantToAct.CurrentHP)
        {
            combatantToAct.AttackTotal = combatantToAct.CurrentHP - 1;
        }
    }

    public override IEnumerator ApplyMoveToSelf()
    {
        yield return TriggerMoveAnimation();
        yield return combatantToAct.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatantToAct);
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.SetTrigger("CombatIdle");
        yield return new WaitForSeconds(1f);
    }

    public override int CalculateAndReturnPotentialChange()
    {
        int dynamicPotentialChange;
        PlayerCombat playerCombat = combatantToAct as PlayerCombat;

        dynamicPotentialChange = (playerCombat.MaxPotential / 4);
        return dynamicPotentialChange;
    }
}
