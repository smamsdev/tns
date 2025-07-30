using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : PreciseMove
{
    public override IEnumerator MoveToPosition(GameObject objectToMove, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.MoveCombatant(objectToMove, targetPosition));
        Destroy(combatMovementInstanceGO);

        Vector3 direction = (combatantToAct.targetToAttack.transform.position - combatantToAct.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        combatantToAct.GetComponent<PlayerMovementScript>().lookDirection = new Vector2(attackDirX, 0);

        yield return new WaitForSeconds(.5f);
    }

    public override Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetToAttack.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

            targetPosition = new Vector3(combatant.targetToAttack.transform.position.x + (targetPositionHorizontalOffset * attackDirX),
                                         combatant.targetToAttack.transform.position.y);

        return targetPosition;
    }
}
