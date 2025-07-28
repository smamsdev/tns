using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : PreciseMove
{
    public override IEnumerator ApplyMove(Combatant player, Combatant targetCombatant)
    {
        Animator combatantToActAnimator = player.GetComponent<Animator>();
        CombatEvents.UpdateNarrator(moveName);

        yield return MoveToPosition(player.gameObject, player.moveSelected.AttackPositionLocation(player));

        Vector3 direction = (player.targetToAttack.transform.position - player.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        player.GetComponent<PlayerMovementScript>().lookDirection = new Vector2(attackDirX, 0);

        yield return new WaitForSeconds(.5f);

        combatManager.cameraFollow.transformToFollow = targetCombatant.transform;
        targetCombatant.combatantUI.fendScript.ApplyAttackToFend(player, player.targetToAttack);

        //start animation

        combatantToActAnimator.SetFloat("MoveAnimationToUse", animtionIntTriggerToUse);
        combatantToActAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);

        //return combatantToAct to fightingpos, and return look direct
        yield return ReturnFromPosition(player.gameObject, player.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle"); //remember to blend the transition in animator settings or it will wiggle

        //reset narrator
        CombatEvents.UpdateNarrator("");
    }

    public override Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x + (targetPositionHorizontalOffset * attackDirX),
                                         combatant.targetToAttack.transform.position.y);

        return targetPosition;
    }

    public override IEnumerator Return()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)
    {
        throw new System.NotImplementedException();
    }
}
