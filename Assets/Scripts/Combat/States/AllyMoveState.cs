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

            var allyToActMovementScript = allyToAct.GetComponent<MovementScript>();
            var allyToActLastLookDirection = allyToActMovementScript.lookDirection;

            var allyToActAnimator = allyToAct.gameObject.GetComponent<Animator>();
            allyToActAnimator.ResetTrigger("CombatIdle");

            //reset narrator focus camera on allyToAct and wait
            CombatEvents.UpdateNarrator("");
            combatManager.cameraFollow.transformToFollow = allyToAct.transform;
            yield return new WaitForSeconds(0.5f);

            var targetToAttackUI = allyToAct.targetToAttack.GetComponentInChildren<CombatantUI>();
            targetToAttackUI.statsDisplay.ShowStatsDisplay(true);
            yield return allyToAct.moveSelected.ApplyMove(allyToAct, allyToAct.targetToAttack);
            allyToAct.GetComponent<MovementScript>().lookDirection = allyToActLastLookDirection;

            //return target to original pos and look dir
            Animator targetToAttackAnimator = allyToAct.targetToAttack.GetComponent<Animator>();
            targetToAttackAnimator.SetTrigger("CombatIdle");
            yield return new WaitForSeconds(0.5f);

            yield return combatManager.PositionCombatant(allyToAct.targetToAttack.gameObject, allyToAct.targetToAttack.fightingPosition.transform.position);
            enemyTargetMovementScript.lookDirection = enemyTargetStoredLookDir;

            //check for player defeat
            if (combatManager.defeat.playerDefeated)
            {
                Debug.Log("player defeated");
                yield break;
            }

            //todo check for enemy defeat
        }
        combatManager.SetState(combatManager.applyMove);
    }
}
