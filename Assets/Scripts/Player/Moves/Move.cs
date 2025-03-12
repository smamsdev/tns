using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public string moveName;
    [TextArea(2, 5)]
    public string moveDescription;
    public bool isEquipped;
    public bool isFlaw;

    [Header("")]
    public float attackMoveMultiplier;
    public float damageToPartsMultiplier;
    public float fendMoveMultiplier;
    public int potentialChange;
    public int moveWeighting;
    public float attackPushStrength;

    [Header("")]
    public bool isAttack;
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public float animtionIntTriggerToUse = 0;
    public float distanceToCover;

    public CombatManager combatManager;

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

    public virtual void LoadMove(Combatant combatant)

    {
        combatant.attackTotal = Mathf.RoundToInt(combatant.attackBase * attackMoveModPercent);
        combatant.fendTotal = Mathf.RoundToInt(combatant.fendBase * fendMoveModPercent);

        var rng = Mathf.RoundToInt(combatant.attackTotal * Random.Range(-0.3f, 0.3f));
        combatant.attackTotal = Mathf.RoundToInt(combatant.attackTotal - combatant.injuryPenalty) + rng;
    }
}
