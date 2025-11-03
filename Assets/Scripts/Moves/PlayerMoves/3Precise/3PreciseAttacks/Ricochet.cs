using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Move
{
    public override IEnumerator MoveToPosition(Combatant combatant, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.LerpPositionFixedTime(combatant.gameObject, targetPosition, .5f));
        Destroy(combatMovementInstanceGO);

        Vector3 direction = (combatantToAct.targetToAttack.transform.position - combatantToAct.transform.position).normalized;
        combatant.CombatLookDirX = (int)Mathf.Sign(direction.x);
    }

    public override Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x + (moveSO.targetPositionHorizontalOffset * attackDirX),
                                         combatant.targetToAttack.transform.position.y);

        return targetPosition;
    }
}
