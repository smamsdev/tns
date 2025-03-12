using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        //   equippedGear = combatManager.player.GetComponent<GearEquip>().equippedGear;
        //   int i;
        //
        //   for (i = 0; i < equippedGear.Length;)
        //
        //   {
        //       equippedGear[i].ApplyFendGear();
        //       i++;
        //   }

        foreach (Enemy enemy in combatManager.enemies)

        {
            //init
            var playerMovementScript = combatManager.player.GetComponent<MovementScript>();
            var storedLookDirection = playerMovementScript.lookDirection;
            var enemyLastLookDirection = enemy.GetComponent<MovementScript>().lookDirection;
            var enemyAnimator = enemy.gameObject.GetComponent<Animator>();
            enemyAnimator.ResetTrigger("CombatIdle");

            //reset narrator
            CombatEvents.UpdateNarrator.Invoke("");


            //begin move
            combatManager.cameraFollow.transformToFollow = enemy.transform;
            yield return new WaitForSeconds(0.5f);
            CombatEvents.UpdateNarrator.Invoke(enemy.moveSelected.moveName);
            yield return enemy.moveSelected.MoveToPosition(enemy.gameObject, enemy.fightingPosition.transform.position);

            //move animation
            enemyAnimator.SetFloat("attackAnimationToUse", enemy.moveSelected.animtionIntTriggerToUse);
            enemyAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);
            //allow half a sec for anim to complete.

            //return to fightingpos, and return look direct
            yield return enemy.moveSelected.ReturnFromPosition(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemyAnimator.SetTrigger("CombatIdle");
            enemy.GetComponent<MovementScript>().lookDirection = enemyLastLookDirection;

            
            CombatEvents.UpdateNarrator.Invoke("");

            //check for death and return player pos

            if (combatManager.defeat.playerDefeated)

            {
                yield break;
            }

            yield return combatManager.PositionCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);
            playerMovementScript.lookDirection = storedLookDirection;
        }

        combatManager.SetState(combatManager.roundReset);
    }
}

