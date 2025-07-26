using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
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
    public float animtionIntTriggerToUse = 0;

    public float targetPositionHorizontalOffset;
    public bool offsetFromSelf;
    public bool applyMoveToSelfOnly;

    [HideInInspector] public CombatManager combatManager;

    public virtual IEnumerator MoveToPosition(GameObject objectToMove, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(objectToMove, targetPosition));
        Destroy(combatMovementInstanceGO);
    }

    public virtual IEnumerator ReturnFromPosition(GameObject objectToMove, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.5f);

        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(objectToMove, targetPosition));
        Destroy(combatMovementInstanceGO);
    }

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

    public virtual void LoadMoveStats(Combatant combatant, CombatManager combatManager)
    {
        this.combatManager = combatManager;

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackBase * attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.fendBase * fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));

        combatant.attackTotal -= combatant.injuryPenalty;
        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal + rng);
    }

    public virtual IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        Animator combatantToActAnimator = combatantToAct.GetComponent<Animator>();
        CombatEvents.UpdateNarrator(moveName);

        if (applyMoveToSelfOnly)
        {
            combatantToActAnimator.SetFloat("MoveAnimationToUse", animtionIntTriggerToUse);
            combatantToActAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
            combatantToActAnimator.SetTrigger("CombatIdle"); //remember to blend the transition in animator settings or it will wiggle
            CombatEvents.UpdateNarrator("");
        }

        else 
        {
            yield return MoveToPosition(combatantToAct.gameObject, combatantToAct.moveSelected.AttackPositionLocation(combatantToAct));

            combatManager.cameraFollow.transformToFollow = targetCombatant.transform;
            targetCombatant.combatantUI.fendScript.ApplyAttackToFend(combatantToAct, combatantToAct.targetToAttack);

            //start animation

            combatantToActAnimator.SetFloat("MoveAnimationToUse", animtionIntTriggerToUse);
            combatantToActAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.2f);

            //return combatantToAct to fightingpos, and return look direct
            yield return ReturnFromPosition(combatantToAct.gameObject, combatantToAct.fightingPosition.transform.position);
            combatantToActAnimator.SetTrigger("CombatIdle"); //remember to blend the transition in animator settings or it will wiggle

            //reset narrator
            CombatEvents.UpdateNarrator("");
        }
    }
}
