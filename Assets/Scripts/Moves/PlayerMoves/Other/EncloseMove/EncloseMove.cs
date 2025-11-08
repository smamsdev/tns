using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncloseMove : Move
{
    public Combatant combatantEnclosed;

    public override IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        combatantEnclosed = targetCombatant;
        combatantEnclosed.isEnclosed = true;

        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, AttackPositionLocation(combatantToAct));
        combatantToActAnimator.SetTrigger("CombatIdle");

        //its simply outrageous to be modifying combatUI elements at Move level but i am very tired
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.tacticalSelectState.lastButtonSelected, Color.white);
        combatManager.tacticalSelectState.lastButtonSelected = combatManager.tacticalSelectState.returnButton;

        yield return new WaitForSeconds(1);
    }

    public override IEnumerator MoveToPosition(Combatant combatant, Vector3 targetPosition)
    {
        var combatMovementInstanceGO = Instantiate(combatManager.combatMovementPrefab, this.transform);
        var combatMovementInstance = combatMovementInstanceGO.GetComponent<CombatMovement>();
        yield return (combatMovementInstance.LerpPositionFixedTime(combatant.gameObject, targetPosition, .5f));
        Destroy(combatMovementInstanceGO);

        Vector3 direction = (combatantToAct.targetCombatant.transform.position - combatantToAct.transform.position).normalized;
        combatant.CombatLookDirX = (int)Mathf.Sign(direction.x);
    }

    public override Vector3 AttackPositionLocation(Combatant combatant)
    {
        Vector3 targetPosition;

        Vector3 direction = (combatant.targetCombatant.transform.position - combatant.transform.position).normalized;
        float attackDirX = Mathf.Sign(direction.x);

        targetPosition = new Vector3(combatant.targetCombatant.transform.position.x + (moveSO.TargetPositionHorizontalOffset * attackDirX),
                                     combatantToAct.targetCombatant.transform.position.y);

        combatantToAct.fightingPosition.transform.position = targetPosition;
        return targetPosition;
    }

}
