using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        foreach (Enemy enemyToAct in combatManager.enemies)
        {
            //store allied target look dir
            var alliedTargetMovementScript = enemyToAct.targetToAttack.GetComponent<MovementScript>();
            var alliedTargetStoredLookDir = alliedTargetMovementScript.lookDirection;

            var enemyLastLookDirection = enemyToAct.GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = enemyToAct.gameObject.GetComponent<Animator>();
            enemyAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on enemy to act and wait
            CombatEvents.UpdateNarrator.Invoke("");
            combatManager.cameraFollow.transformToFollow = enemyToAct.transform;
            yield return new WaitForSeconds(0.5f);

            //display move name and move to position
            CombatEvents.UpdateNarrator.Invoke(enemyToAct.moveSelected.moveName);
            yield return enemyToAct.moveSelected.MoveToPosition(enemyToAct.gameObject, enemyToAct.moveSelected.AttackPositionLocation(enemyToAct));

            //apply move effects to allied target
            var targetToAttackUI = enemyToAct.targetToAttack.GetComponentInChildren<FendScript>();
            targetToAttackUI.ApplyAttackToFend(enemyToAct, enemyToAct.targetToAttack);

            combatManager.cameraFollow.transformToFollow = enemyToAct.targetToAttack.transform;
            
            //StartCoroutine(combatManager.selectedPlayerMove.OnEnemyAttack(combatManager, enemyToAct));

            //start animation
            enemyAnimator.SetFloat("attackAnimationToUse", enemyToAct.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);

            //return enemy to fightingpos, and return look direct
            yield return enemyToAct.moveSelected.ReturnFromPosition(enemyToAct.gameObject, enemyToAct.fightingPosition.transform.position);
            enemyAnimator.SetTrigger("CombatIdle");
            enemyToAct.GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            //reset narrator
            CombatEvents.UpdateNarrator.Invoke("");

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //return allied target to original pos and look dir
            yield return new WaitForSeconds(0.5f);
            yield return combatManager.PositionCombatant(enemyToAct.targetToAttack.gameObject, enemyToAct.targetToAttack.fightingPosition.transform.position);
            alliedTargetMovementScript.lookDirection = alliedTargetStoredLookDir;
        }
        combatManager.SetState(combatManager.roundReset);
    }
}