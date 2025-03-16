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
    public bool targetIsSelf;

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
        float lookDirX = combatant.GetComponent<MovementScript>().lookDirection.x;

        if (targetIsSelf)
        {
            targetPosition = new Vector3(combatant.transform.position.x + (targetPositionHorizontalOffset * lookDirX), combatant.transform.position.y);
        }

        else
        {
            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x - (targetPositionHorizontalOffset * lookDirX), combatant.targetToAttack.transform.position.y);
        }

        return targetPosition;
    }

    public void LoadMoveStats(Combatant combatant, CombatManager combatManager)
    {
        this.combatManager = combatManager;

        combatant.attackTotal = Mathf.RoundToInt(combatant.attackBase * attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.fendBase * fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));

        combatant.attackTotal -= combatant.injuryPenalty;
        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal + rng);
    }
}
