using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnMove : Move
{
    [SerializeField] EncloseMove encloseMove;

    public override IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        encloseMove.combatantEnclosed.isEnclosed = false;

        combatantToActAnimator.Play("Advance");
        yield return MoveToPosition(combatantToAct, combatantToAct.fightingPosition.transform.position);
        combatantToActAnimator.SetTrigger("CombatIdle");
        Vector3 direction = (combatantToAct.targetCombatant.transform.position - combatantToAct.transform.position).normalized;
        combatantToAct.CombatLookDirX = (int)Mathf.Sign(direction.x);

        yield return new WaitForSeconds(1);
    }
}
