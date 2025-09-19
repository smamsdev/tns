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
        yield return MoveToPosition(combatantToAct.gameObject, AttackPositionLocation(combatantToAct));

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
            enemy.combatantUI.statsDisplay.ShowStatsDisplay(true);
            enemy.combatantUI.fendScript.ApplyAttackToFend(combatantToAct, enemy, (combatantToAct.attackTotal * 3));
        }

        yield return new WaitForSeconds(.5f);
        combatantToActAnimator.Play("Back");

        //return combatantToAct to fightingpos, and return look direct
        yield return ReturnFromPosition(combatantToAct.gameObject, combatantToAct.fightingPosition.transform.position);

        foreach (Enemy enemy in enemiesInFront)
        {
            enemy.combatantUI.statsDisplay.ShowStatsDisplay(false);
        }

        combatantToActMovementScript.lookDirection = playerDefaultLookDirection;
        TriggerIdleAnimation();

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

            else         //return target to original pos if still alive
            {
                yield return new WaitForSeconds(0.5f);
                yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            }
        }
    }
}
