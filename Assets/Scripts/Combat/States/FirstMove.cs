using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject firstMoveContainer;

    public override IEnumerator StartState()
    {
        combatManager.playerCombat.combatantUI.fendScript.animator.SetBool("fendBreak", false);

        yield return new WaitForSeconds(0.1f);

        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.firstMoveMenu);
        combatManager.combatMenuManager.firstMenuFirstButton.Select();
        combatManager.playerMoveManager.firstMoveIs = 0;

        yield break;
    }

    public override void CombatOptionSelected (int moveValue)
    {
        combatManager.playerMoveManager.firstMoveIs = moveValue;
        string moveName = "";

        switch (moveValue)
        {
            case 1:
                moveName = "Violent";
                break;
            case 2:
                moveName = "Cautious";
                break;
            case 3:
                moveName = "Precise";
                break;
            case 4:
                moveName = "Organised";
                break;
        }

        if (moveValue == 4)
        {
            combatManager.SetState(combatManager.gearSelect);
        }
        else
        {
            combatManager.SetState(combatManager.secondMove);
        }
    }
}