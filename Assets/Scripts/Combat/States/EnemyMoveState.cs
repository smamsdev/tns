using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        foreach (Enemy enemy in combatManager.enemies)
        {
            //store player look dir
            var playerMovementScript = combatManager.player.GetComponent<MovementScript>();
            var storedLookDirection = playerMovementScript.lookDirection;

            var enemyLastLookDirection = enemy.GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = enemy.gameObject.GetComponent<Animator>();
            enemyAnimator.ResetTrigger("CombatIdle");

            //reset narrator
            CombatEvents.UpdateNarrator.Invoke("");

            //focus camera on enemy and wait
            combatManager.cameraFollow.transformToFollow = enemy.transform;
            yield return new WaitForSeconds(0.5f);

            //display move name and move to position
            CombatEvents.UpdateNarrator.Invoke(enemy.moveSelected.moveName);

            yield return enemy.moveSelected.MoveToPosition(enemy.gameObject, enemy.moveSelected.AttackPositionLocation(enemy));

            //start animation
            enemyAnimator.SetFloat("attackAnimationToUse", enemy.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);

            //return enemy to fightingpos, and return look direct
            yield return enemy.moveSelected.ReturnFromPosition(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemyAnimator.SetTrigger("CombatIdle");
            enemy.GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            //reset narrator
            CombatEvents.UpdateNarrator.Invoke("");

            //check for player defeat
            if (combatManager.defeat.playerDefeated)

            {
                Debug.Log("player defeated");
                yield break;
            }

            //return player to original pos and look dir
            yield return combatManager.PositionCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);
            playerMovementScript.lookDirection = storedLookDirection;
        }

        combatManager.SetState(combatManager.roundReset);
    }
}

