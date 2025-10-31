using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMove : PlayerMove
{
    [SerializeField] EncloseMove encloseMove;

    public override IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        GetReferences(combatantToAct, targetCombatant);
        UpdateNarrator(moveName);
        encloseMove.combatantEnclosed.isEnclosed = false;

        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.tacticalSelectState.lastButtonSelected, Color.white);
        combatManager.tacticalSelectState.lastButtonSelected = combatManager.tacticalSelectState.gearButton;


        Vector3 direction = (combatantToAct.targetToAttack.transform.position - combatantToAct.transform.position).normalized;
        combatantToAct.CombatLookDirX = (int)Mathf.Sign(direction.x);

        yield return new WaitForSeconds(1);
    }
}
