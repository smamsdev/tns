using System.Collections;
using UnityEngine;

public class RecklessAttackBehaviour : MoveBehaviour
{
    public override IEnumerator ApplyMoveToEnemy()
    {
        combatantToAct.targetCombatant.combatantUI.fendScript.ShowFendDisplay(combatantToAct.targetCombatant, true);
        yield return new WaitForSeconds(0.5f);

        //move to attack pos
        combatantToActAnimator.Play("Advance");
        StartCoroutine(combatantToAct.CombatHPChanged(combatantToAct.AttackTotal / -2, combatManager));

        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));

        if (combatantToAct.CurrentHP <= 0)
        {
            combatantToAct.targetCombatant.combatantUI.fendScript.ShowFendDisplay(combatantToAct.targetCombatant, false);
            yield return new WaitForSeconds(1f);
            yield break;
        }

        CheckForBackStabBonus();

        //apply stats to enemy and animate
        yield return TriggerMoveAnimation();
        yield return ApplyAttackToTarget();
        combatantToAct.targetCombatant.combatantUI.fendScript.ShowFendDisplay(combatantToAct.targetCombatant, false);

        //return combatantToAct to fightingpos
        combatantToActAnimator.Play("Back");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return ReturnTargetToFightingPos();
        combatantToAct.targetCombatant.isBackstabbed = false;
    }
}
