using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public MoveSO moveSO;
    public CombatManager combatManager;
    protected Animator combatantToActAnimator;
    public MovementScript combatantToActMovementScript;
    public Combatant combatantToAct, targetCombatant;

    public virtual IEnumerator MoveToPosition(Combatant combatant, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.LerpPositionBySpeed(combatant.gameObject, targetPosition, combatant.movementScript.movementSpeed));
        Destroy(combatMovementInstanceGO);
    }

    public virtual Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetCombatant.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        if (moveSO.OffsetFromSelf)
        {
            targetPosition = new Vector3(combatant.transform.position.x + (moveSO.TargetPositionHorizontalOffset * attackDirX),
                                         combatant.transform.position.y);
        }
        else
        {
            targetPosition = new Vector3(combatant.targetCombatant.transform.position.x - (moveSO.TargetPositionHorizontalOffset * attackDirX),
                                         combatant.targetCombatant.transform.position.y);
        }

        return targetPosition;
    }

    public virtual void LoadMoveReferences(Combatant combatantToAct, CombatManager combatManager)
    {
        this.combatManager = combatManager;
        combatantToActAnimator = combatantToAct.GetComponent<Animator>();
        combatantToActMovementScript = combatantToAct.GetComponent<MovementScript>();
        this.combatantToAct = combatantToAct;
        this.targetCombatant = combatantToAct.targetCombatant;
    }

    public virtual void CalculateMoveStats()
    {
        Debug.Log(combatantToAct.name + combatantToAct.AttackBase);
        combatantToAct.AttackTotal = Mathf.RoundToInt(combatantToAct.AttackBase * moveSO.AttackMoveModPercent);
        combatantToAct.FendTotal = Mathf.RoundToInt(combatantToAct.FendBase * moveSO.FendMoveModPercent);

        var rng = Mathf.RoundToInt(combatantToAct.AttackTotal * Random.Range(-0.3f, 0.3f));

        combatantToAct.AttackTotal = Mathf.RoundToInt(combatantToAct.AttackTotal + rng);
    }

    public virtual IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        if (moveSO.ApplyMoveToSelfOnly)
        {
            yield return ApplyMoveToSelf();
        }

        else
        {
            Vector3 direction = (combatantToAct.targetCombatant.transform.position - combatantToAct.transform.position).normalized;
            combatantToAct.CombatLookDirX = (int)Mathf.Sign(direction.x);
            yield return ApplyMoveToEnemy();
        }
    }

    public virtual IEnumerator TriggerMoveAnimation()
    {
        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveSO.MoveAnimationFloat);
        combatantToActAnimator.Play("Attack");
        yield return null;
    }

    public virtual IEnumerator ApplyMoveToSelf()
    {
        yield return TriggerMoveAnimation();
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.SetTrigger("CombatIdle");
        yield return new WaitForSeconds(1f);
    }

    public virtual IEnumerator ApplyMoveToEnemy()
    {
        targetCombatant.combatantUI.fendScript.ShowFendDisplay(targetCombatant, true);
        yield return new WaitForSeconds(0.5f);

        //move to attack pos
        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));

        //set backstab status
        if (combatantToAct.CombatLookDirX == targetCombatant.CombatLookDirX)
        {
            targetCombatant.isBackstabbed = true;
        }

        //apply stats to enemy and animate
        combatManager.cameraFollow.transformToFollow = targetCombatant.transform;

        yield return TriggerMoveAnimation();
        yield return targetCombatant.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatantToAct.targetCombatant);

        combatantToActAnimator.Play("Back");

        //return combatantToAct to fightingpos
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");

        yield return ReturnTargetToFightingPos();
        targetCombatant.isBackstabbed = false;
    }

    public virtual IEnumerator ReturnTargetToFightingPos()
    {
        if (combatantToAct.targetCombatant.CurrentHP == 0)
        {
            combatManager.CombatantDefeated(combatantToAct.targetCombatant);
            yield return new WaitForSeconds(1.5f);
        }

        else //return target to original pos if still alive
        {
            combatantToAct.targetCombatant.combatantUI.statsDisplay.ShowStatsDisplay(false);

            if (targetCombatant.isBackstabbed)
            {
                combatantToAct.targetCombatant.movementScript.animator.Play("Back");
            }

            else
            {
                combatantToAct.targetCombatant.movementScript.animator.Play("Advance");
            }

            yield return combatManager.PositionCombatant(combatantToAct.targetCombatant.gameObject, combatantToAct.targetCombatant.fightingPosition.transform.position);
            combatantToAct.targetCombatant.movementScript.animator.Play("CombatIdle");
        }
    }

    public virtual int CalculateAndReturnPotentialChange()
    {
        return moveSO.PotentialChange;
    }

    // Optional for reactions to attacks. Default does nothing.
    public virtual IEnumerator OnReceieveAttack(Combatant combatantApplying, Combatant combatantReceiving)
    {
        yield break;
    }
}
