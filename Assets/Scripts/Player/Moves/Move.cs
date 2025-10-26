using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;
    public bool isEquipped;

    [Header("")]
    public int moveWeighting;
    public float attackPushStrength;

    [Header("")]
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public float moveAnimationFloat = 0;

    public float targetPositionHorizontalOffset;
    public bool offsetFromSelf;
    public bool applyMoveToSelfOnly;

    [HideInInspector] public CombatManager combatManager;
    [HideInInspector] public Animator combatantToActAnimator;
    [HideInInspector] public MovementScript combatantToActMovementScript;
    [HideInInspector] public Combatant combatantToAct, targetCombatant;

    public virtual IEnumerator MoveToPosition(Combatant combatant, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.LerpPositionBySpeed(combatant.gameObject, targetPosition, combatant.movementScript.movementSpeed));
        Destroy(combatMovementInstanceGO);
    }

   // public virtual IEnumerator ReturnFromPosition(GameObject objectToMove, Vector3 targetPosition)
   // {
   //     var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
   //     var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
   //     yield return (combatMovementInstance.MoveCombatant(objectToMove, targetPosition));
   //     Destroy(combatMovementInstanceGO);
   // }

    public virtual Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        if (offsetFromSelf)
        {
            targetPosition = new Vector3(combatant.transform.position.x + (targetPositionHorizontalOffset * attackDirX),
                                         combatant.transform.position.y);
        }
        else
        {
            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x - (targetPositionHorizontalOffset * attackDirX),
                                         combatant.targetToAttack.transform.position.y);
        }

        return targetPosition;
    }

    public virtual void LoadMoveStatsAndPassCBM(Combatant combatant, CombatManager combatManager)
    {
        this.combatManager = combatManager;

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackBase * attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.fendBase * fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal + rng);
    }

    public virtual IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        GetReferences(combatantToAct, targetCombatant);
        UpdateNarrator(moveName);

        if (applyMoveToSelfOnly)
        {
            yield return ApplyMoveToSelf();
        }

        else
        {
            combatantToActAnimator.Play("Advance");
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

    public virtual void TriggerMoveAnimation()
    {
        combatantToActAnimator.SetFloat("MoveAnimationFloat", moveAnimationFloat);
        combatantToActAnimator.Play("Attack");
    }

    public virtual IEnumerator ApplyMoveToSelf()
    {
        TriggerMoveAnimation();
        yield return new WaitForSeconds(0.5f);
        combatantToActAnimator.SetTrigger("CombatIdle");
        yield return new WaitForSeconds(1f);
        UpdateNarrator("");
    }

    public virtual IEnumerator ApplyMoveToEnemy()
    {
        var playerDefaultLookDirection = combatantToActMovementScript.lookDirection;

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

        //set backstab status
        if (combatantToAct.CombatLookDirX == targetCombatant.CombatLookDirX)
        {
            targetCombatant.isBackstabbed = true;
        }

        //apply stats to enemy and animate
        combatManager.cameraFollow.transformToFollow = targetCombatant.transform;

        var spriteRenderer = combatantToActAnimator.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        TriggerMoveAnimation();
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
            yield return new WaitForSeconds(0.5f);
            combatManager.CombatantDefeated(combatantToAct.targetToAttack);
            combatantToAct.targetToAttack.combatantUI.statsDisplay.statsDisplayContainerAnimator.Play("StatsDisplayOnDefeat");
            yield return new WaitForSeconds(1.5f);
        }

        else //return target to original pos if still alive
        {
            yield return new WaitForSeconds(0.5f);
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
