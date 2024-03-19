using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject firstMoveContainer;

    public float testLerp = 0;

    public override IEnumerator StartState()
    {
        CombatEvents.SendMove += SetFirstMove;

        yield return new WaitForSeconds(0.1f);
        firstMoveContainer.SetActive(true);

        combatManager.combatUIScript.ShowFirstMoveMenu();
        combatManager.playerMoveManager.firstMoveIs = 0;

        combatManager.combatUIScript.playerFendScript.ShowHideFendDisplay(true);

        //   // Example: Lerp testLerp from 0 to 10 over 3 seconds
        //   StartCoroutine(Lerper.LerpFloat(testLerp, 10f, 3f, (lerpedValue) =>
        //   {
        //       testLerp = lerpedValue; // Update testLerp each frame
        //   }));

        yield break;
    }

    void SetFirstMove(int moveValue)

    {
        combatManager.playerMoveManager.firstMoveIs = moveValue;
        string moveName = "";

        switch (moveValue)

        { 
            case 1: moveName = "Violent";
                break;
            case 2:
                moveName = "Cautious";
                break;
            case 3:
                moveName = "Precise";
                break; 
        };

        CombatEvents.UpdateFirstMoveDisplay.Invoke(moveName);
        CombatEvents.SendMove -= SetFirstMove;
    }
}
