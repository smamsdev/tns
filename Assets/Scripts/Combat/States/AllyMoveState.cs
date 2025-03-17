using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMoveState : State
{
    [SerializeField] CombatManager combatManager;

public override IEnumerator StartState()
{

    foreach (Ally allyToAct in combatManager.allies)
    {
        //store enemy target look dir
        var enemyTargetMovementScript = allyToAct.targetToAttack.GetComponent<MovementScript>();
        var enemyTargetStoredLookDir = enemyTargetMovementScript.lookDirection;

        var allyToActLastLookDirection = allyToAct.GetComponent<MovementScript>().lookDirection;
        var allyToActAnimator = allyToAct.gameObject.GetComponent<Animator>();
        allyToActAnimator.ResetTrigger("CombatIdle");

        //reset narrator focus camera on enemy to act and wait
        CombatEvents.UpdateNarrator.Invoke("");
        combatManager.cameraFollow.transformToFollow = allyToAct.transform;
        yield return new WaitForSeconds(0.5f);

        //display move name and move to position
        CombatEvents.UpdateNarrator.Invoke(allyToAct.moveSelected.moveName);
        yield return allyToAct.moveSelected.MoveToPosition(allyToAct.gameObject, allyToAct.moveSelected.AttackPositionLocation(allyToAct));

        //apply move effects to allied target
        var targetToAttackUI = allyToAct.targetToAttack.GetComponentInChildren<FendScript>();
        targetToAttackUI.ApplyAttackToFend(allyToAct, allyToAct.targetToAttack);

        //start animation
        allyToActAnimator.SetFloat("attackAnimationToUse", allyToAct.moveSelected.animtionIntTriggerToUse);
        allyToActAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);

        //return enemy to fightingpos, and return look direct
        yield return allyToAct.moveSelected.ReturnFromPosition(allyToAct.gameObject, allyToAct.fightingPosition.transform.position);
        allyToActAnimator.SetTrigger("CombatIdle");
        allyToAct.GetComponent<MovementScript>().lookDirection = allyToActLastLookDirection;


        //reset narrator
        CombatEvents.UpdateNarrator.Invoke("");

        //check for player defeat
        if (combatManager.defeat.playerDefeated)
        {
            Debug.Log("player defeated");
            yield break;
        }

        //return enemy target to original pos and look dir
        yield return new WaitForSeconds(0.5f);
        yield return combatManager.PositionCombatant(allyToAct.targetToAttack.gameObject, allyToAct.targetToAttack.fightingPosition.transform.position);
        enemyTargetMovementScript.lookDirection = enemyTargetStoredLookDir;
    }
    
        combatManager.SetState(combatManager.applyMove);
   }
}
