using System.Collections;
using UnityEngine;

public class WithdrawBehaviour : MoveBehaviour
{
    public override IEnumerator ApplyMoveToSelf()
    {
        //move to pos
        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));
        combatantToAct.CombatLookDirX = -combatantToAct.CombatLookDirX;
        yield return TriggerMoveAnimation();

        yield return new WaitForSeconds(0.5f);
        //return combatantToAct to fightingpos
        combatantToActAnimator.Play("Back");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");
    }

    public override int CalculateAndReturnPotentialChange()
    {
        PlayerCombat playerCombat = combatantToAct as PlayerCombat;
        int potChange = playerCombat.CurrentPotential;
        return potChange;
    }
}
