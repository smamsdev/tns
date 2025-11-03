using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
    protected MoveSO moveSO;
    protected CombatManager combatManager;
    protected Animator combatantToActAnimator;
    protected MovementScript combatantToActMovementScript;
    protected Combatant combatantToAct, targetCombatant;

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

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        if (moveSO.offsetFromSelf)
        {
            targetPosition = new Vector3(combatant.transform.position.x + (moveSO.targetPositionHorizontalOffset * attackDirX),
                                         combatant.transform.position.y);
        }
        else
        {
            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x - (moveSO.targetPositionHorizontalOffset * attackDirX),
                                         combatant.targetToAttack.transform.position.y);
        }

        return targetPosition;
    }

    public virtual void LoadMoveStatsAndPassCBM(Combatant combatant, CombatManager combatManager)
    {
        this.combatManager = combatManager;

        combatant.attackTotal = Mathf.RoundToInt(combatant.AttackBase * moveSO.attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.FendBase * moveSO.fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal + rng);
    }

    public virtual IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        GetReferences(combatantToAct, targetCombatant);
        UpdateNarrator(moveSO.moveName);

        if (moveSO.applyMoveToSelfOnly)
        {
            yield return ApplyMoveToSelf();
        }

        else
        {
            Vector3 direction = (combatantToAct.targetToAttack.transform.position - combatantToAct.transform.position).normalized;
            combatantToAct.CombatLookDirX = (int)Mathf.Sign(direction.x);
            yield return ApplyMoveToEnemy();
        }
    }

    public virtual void GetReferences(Combatant combatantToAct, Combatant targetCombatant)
    {
        combatantToActAnimator = combatantToAct.GetComponent<Animator>();
        combatantToActMovementScript = combatantToAct.GetComponent<MovementScript>();
        this.combatantToAct = combatantToAct;
        this.targetCombatant = targetCombatant;
    }

    public virtual void UpdateNarrator(string narratorString)
    {
        CombatEvents.UpdateNarrator(narratorString);
    }

    public virtual IEnumerator TriggerMoveAnimation()
    {
        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveSO.moveAnimationFloat);
        combatantToActAnimator.Play("Attack");
        yield return null;
    }

    public virtual IEnumerator ApplyMoveToSelf()
    {
        yield return TriggerMoveAnimation();
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.SetTrigger("CombatIdle");
        yield return new WaitForSeconds(1f);
        UpdateNarrator("");
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

        var spriteRenderer = combatantToActAnimator.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        yield return TriggerMoveAnimation();
        yield return targetCombatant.combatantUI.fendScript.ApplyAttackToCombatant(combatantToAct, combatantToAct.targetToAttack);

        spriteRenderer.sortingOrder = 0;

        combatantToActAnimator.Play("Back");

        //return combatantToAct to fightingpos
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");

        UpdateNarrator("");

        yield return ReturnTargetToFightingPos();
        targetCombatant.isBackstabbed = false;
    }

    public virtual IEnumerator ReturnTargetToFightingPos()
    {
        if (combatantToAct.targetToAttack.CurrentHP == 0)
        {
            combatManager.CombatantDefeated(combatantToAct.targetToAttack);
            yield return new WaitForSeconds(1.5f);
        }

        else //return target to original pos if still alive
        {
            combatantToAct.targetToAttack.combatantUI.statsDisplay.ShowStatsDisplay(false);

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

    // Optional for reactions to attacks. Default does nothing.
    public virtual IEnumerator OnReceieveAttack(Combatant combatantApplying, Combatant combatantReceiving)
    {
        yield break;
    }
}
