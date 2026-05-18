using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imitator : MoveBehaviour
{
    public override IEnumerator ApplyMove(Combatant combatantToAct, Combatant targetCombatant)
    {
        Debug.Log("ive probably broken this");

        yield return ApplyMoveToSelf();

        combatantToAct.InstantiateMoveBehaviour(combatantToAct.targetCombatant.currentMoveBehaviour.moveSO);

        combatManager.cameraFollow.transformToFollow = combatantToAct.transform;
        yield return new WaitForSeconds(0.25f);

        combatantToAct.currentMoveBehaviour.LoadMoveReferences(combatantToAct, combatManager);
        combatManager.combatMenuManager.UpdateNarrator(combatantToAct.currentMoveBehaviour.moveSO.MoveName);

        yield return new WaitForSeconds(1f);
        combatantToAct.currentMoveBehaviour.CalculateMoveStats();
        combatManager.combatMenuManager.UpdateNarrator("");

        //rock out
        yield return combatantToAct.currentMoveBehaviour.ApplyMove(combatantToAct, combatantToAct.targetCombatant);
    }
}
