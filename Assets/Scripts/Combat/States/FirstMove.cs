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

        combatManager.combatUIScript.playerFendScript.animator.SetBool("fendbreak", false);

        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.ShowFirstMoveMenu(true);
        combatManager.playerMoveManager.firstMoveIs = 0;

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

        if (moveValue == 4)

        {
            combatManager.SetState(combatManager.gearSelect);
        }

        else
        {
            combatManager.SetState(combatManager.secondMove);
        }

        combatManager.combatUIScript.UpdateFirstMoveDisplay(moveName);
        CombatEvents.SendMove -= SetFirstMove;
    }

    private void OnDisable()
    {
        CombatEvents.SendMove -= SetFirstMove;
    }
}
