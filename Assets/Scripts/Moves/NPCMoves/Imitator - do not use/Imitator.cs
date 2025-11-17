using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitator : Move
{
    public override IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        yield return ApplyMoveToSelf();

        combatantToAct.moveSelected = combatantToAct.targetCombatant.moveSelected;

        combatManager.cameraFollow.transformToFollow = combatantToAct.transform;
        yield return new WaitForSeconds(0.25f);

        combatantToAct.moveSelected.LoadMoveReferences(combatantToAct, combatManager);
        CombatEvents.UpdateNarrator(combatantToAct.moveSelected.moveSO.MoveName);

        yield return new WaitForSeconds(1f);
        combatantToAct.moveSelected.CalculateMoveStats();
        CombatEvents.UpdateNarrator("");

        //rock out
        yield return combatantToAct.moveSelected.ApplyMove(combatantToAct, combatantToAct.targetCombatant);
    }
}
