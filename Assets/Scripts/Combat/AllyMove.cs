using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllyMove : MonoBehaviour
{
    public float attackMoveModPercent;
    public float fendMoveModPercent;
    public int moveWeighting;
    public float animtionIntTriggerToUse = 0;
    public float distanceToCoverPercent = 80f;
    public float attackPushStrength = 0.2f;
    public string moveName;
    [HideInInspector] public CombatManager combatManager;
    [HideInInspector] public Ally ally;

    public abstract IEnumerator AllyAttack(CombatManager _combatManager);

    public virtual IEnumerator AllyReturn()

    {
        yield return new WaitForSeconds(0.5f);

        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        Debug.Log(combatMovementInstance);
        yield return (combatMovementInstance.MoveCombatant(ally.gameObject, ally.fightingPosition.transform.position));
        Destroy(combatMovementInstanceGO);
    }

    public abstract void LoadMove(Ally ally);


}
