using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Move
{
    public override IEnumerator MoveToPosition(Combatant combatant, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.LerpPositionFixedTime(combatant.gameObject, targetPosition, .5f));
        Destroy(combatMovementInstanceGO);

        Vector3 direction = (combatantToAct.targetCombatant.transform.position - combatantToAct.transform.position).normalized;
        combatant.CombatLookDirX = (int)Mathf.Sign(direction.x);
    }

    public override Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetCombatant.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

            targetPosition = new Vector3(combatant.targetCombatant.transform.position.x + (moveSO.TargetPositionHorizontalOffset * attackDirX),
                                         combatant.targetCombatant.transform.position.y);

        return targetPosition;
    }

    //SPECIAL - hang out behind enemy for a bit while their fend shows, before applying
    public override IEnumerator ApplyMoveToEnemy()
    {
        //move to attack pos
        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));
        combatantToActAnimator.SetTrigger("CombatIdle");

        targetCombatant.combatantUI.fendScript.ShowFendDisplay(targetCombatant, true);
        yield return new WaitForSeconds(0.5f);

        //set backstab status
        if (combatantToAct.CombatLookDirX == targetCombatant.CombatLookDirX)
        {
            targetCombatant.isBackstabbed = true;
        }

        //apply stats to enemy and animate
        combatManager.cameraFollow.transformToFollow = targetCombatant.transform;

        yield return TriggerMoveAnimation();
        yield return ApplyAttackToTarget();
        targetCombatant.combatantUI.fendScript.ShowFendDisplay(targetCombatant, false);

        //return combatantToAct to fightingpos
        combatantToActAnimator.Play("Back");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return ReturnTargetToFightingPos();
        targetCombatant.isBackstabbed = false;
    }
}
