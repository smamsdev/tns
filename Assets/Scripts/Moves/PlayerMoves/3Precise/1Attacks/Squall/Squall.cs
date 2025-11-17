using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squall : Move
{
    List<Combatant> targetsInFront = new List<Combatant>();
    List<Combatant> targetList;


    public override IEnumerator ApplyMoveToEnemy()
    {
        targetsInFront.Clear();

        //move to attack pos
        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));

        if (combatantToAct is Enemy)
        { 
            targetList = combatManager.allAlliesToTarget; 
        }

        else targetList = combatManager.enemies;

        foreach (Combatant combatant in targetList)
        {
            float dirToTargetX = combatant.transform.position.x - combatantToAct.transform.position.x;

            if (Mathf.Sign(dirToTargetX) == Mathf.Sign(combatantToAct.CombatLookDirX))
            {
                targetsInFront.Add(combatant);
            }
        }

        foreach (Combatant combatant in targetsInFront)
        {
            combatant.combatantUI.fendScript.ShowFendDisplay(combatant, true);
        }

        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return new WaitForSeconds(.5f);

        yield return TriggerMoveAnimation();

        foreach (Combatant combatant in targetsInFront)
        {
            StartCoroutine(combatant.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatant));
        }

        yield return new WaitForSeconds(1.5f);

        foreach (Combatant combatant in targetsInFront)
        {
            combatant.combatantUI.fendScript.ShowFendDisplay(combatant, false);
        }

        //return combatantToAct to fightingpos
        combatantToActAnimator.Play("Back");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);

        foreach (Combatant combatant in targetsInFront)
        {
            combatant.combatantUI.statsDisplay.ShowStatsDisplay(false);
        }

        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return ReturnTargetToFightingPos();
    }

    public override IEnumerator ReturnTargetToFightingPos()
    {
        foreach (Combatant combatant in targetsInFront)
        {
            if (combatant.CurrentHP == 0)
            {
                combatManager.CombatantDefeated(combatant);
                yield return new WaitForSeconds(1.5f);
            }

            else //return target to original pos if still alive
            {
                yield return new WaitForSeconds(0.5f);

                if (targetCombatant.isBackstabbed)
                {
                    combatant.movementScript.animator.Play("Back");
                }

                else
                {
                    combatant.movementScript.animator.Play("Advance");
                }

                yield return combatManager.PositionCombatant(combatant.gameObject, combatant.fightingPosition.transform.position);
                combatant.movementScript.animator.Play("CombatIdle");
            }
        }
    }

    public override IEnumerator TriggerMoveAnimation()
    {
        float moveAnimationFloat = 0;

        if (combatantToAct is PlayerCombat)
        {
            moveAnimationFloat = moveSO.MoveAnimationFloat;
        }

        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveAnimationFloat);

        combatantToActAnimator.speed = 0;
        combatantToActAnimator.Play("Attack", 0, 0);
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.Play("Attack", 0, 0.2f);
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.Play("Attack", 0, 0.7f);
        combatantToActAnimator.speed = 1;
    }
}
