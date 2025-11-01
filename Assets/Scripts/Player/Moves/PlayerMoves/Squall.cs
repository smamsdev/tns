using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squall : ViolentMove
{
    public List<Enemy> enemiesInFront = new List<Enemy>();

    public override IEnumerator ApplyMoveToEnemy()
    {
        enemiesInFront.Clear();

        //move to attack pos
        combatantToActAnimator.Play("Advance");
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
            enemy.combatantUI.fendScript.ShowFendDisplay(enemy, true);
        }

        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return new WaitForSeconds(.5f);

        yield return TriggerMoveAnimation();

        foreach (Enemy enemy in enemiesInFront)
        {
            StartCoroutine(enemy.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, enemy));
        }

        yield return new WaitForSeconds(.5f);

        //return combatantToAct to fightingpos
        combatantToActAnimator.Play("Back");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);

        foreach (Enemy enemy in enemiesInFront)
        {
            enemy.combatantUI.statsDisplay.ShowStatsDisplay(false);
        }

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
                combatManager.CombatantDefeated(enemy);
                yield return new WaitForSeconds(1.5f);
            }

            else //return target to original pos if still alive
            {
                yield return new WaitForSeconds(0.5f);

                if (targetCombatant.isBackstabbed)
                {
                    enemy.movementScript.animator.Play("Back");
                }

                else
                {
                    enemy.movementScript.animator.Play("Advance");
                }

                yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
                enemy.movementScript.animator.Play("CombatIdle");
            }
        }
    }
}
