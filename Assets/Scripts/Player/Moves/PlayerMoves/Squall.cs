using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squall : ViolentMove
{
    public List<Enemy> enemiesInFront = new List<Enemy>();

    public override IEnumerator ApplyMoveToEnemy()
    {
        var playerDefaultLookDirection = combatantToActMovementScript.lookDirection;
        var targetToAttackUI = combatantToAct.targetToAttack.GetComponentInChildren<CombatantUI>();
        enemiesInFront.Clear();

        //move to attack pos
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));

        //move counterattack?
        if (combatantToAct.targetToAttack.moveSelected != null)
        {
            yield return combatantToAct.targetToAttack.moveSelected.OnReceieveAttack(combatantToAct, combatantToAct.targetToAttack);
        }

        if (combatantToAct.CurrentHP == 0)
        {
            yield break;
        }

        //apply stats to enemy and animate
        combatManager.cameraFollow.transformToFollow = targetCombatant.transform;
        TriggerMoveAnimation();

        foreach (Enemy enemy in combatManager.enemies)
        {
            float dirToEnemyX = enemy.transform.position.x - combatManager.playerCombat.transform.position.x;

            if (Mathf.Sign(dirToEnemyX) == Mathf.Sign(combatManager.playerCombat.movementScript.lookDirection.x))
            {
                enemiesInFront.Add(enemy);
            }
        }

        foreach (Enemy enemy in enemiesInFront)
        {
            yield return enemy.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, enemy);
        }

        yield return new WaitForSeconds(.5f);
        combatantToActAnimator.SetFloat("lookDirectionX", -1);
        combatantToActAnimator.Play("Back");

        //return combatantToAct to fightingpos, and return look direct
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);

        foreach (Enemy enemy in enemiesInFront)
        {
            enemy.combatantUI.statsDisplay.ShowStatsDisplay(false);
        }

        combatantToActMovementScript.lookDirection = playerDefaultLookDirection;
        combatantToActAnimator.SetTrigger("CombatIdle");

        UpdateNarrator("");

        yield return ReturnTargetToFightingPos();
    }

    public override IEnumerator ReturnTargetToFightingPos()
    {
        foreach (Enemy enemy in enemiesInFront)
        {

            if (enemy.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatantToAct.targetToAttack);
            }

            else //return target to original pos if still alive
            {
                yield return new WaitForSeconds(0.5f);

                if (targetCombatant.isBackstabbed)
                {
                    combatantToAct.targetToAttack.movementScript.animator.Play("Back");
                }

                else
                {
                    combatantToAct.targetToAttack.movementScript.animator.Play("Advance");
                }

                yield return combatManager.PositionCombatant(combatantToAct.targetToAttack.gameObject, combatantToAct.targetToAttack.fightingPosition.transform.position);
                combatantToAct.targetToAttack.movementScript.animator.Play("CombatIdle");
            }
        }
    }
}
