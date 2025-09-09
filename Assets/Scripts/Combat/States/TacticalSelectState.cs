using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalSelectState : State
{
    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, true);
        combatManager.combatMenuManager.tacticalSelectMenuDefaultButton.Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, false);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.lastButtonSelected, Color.white);
            combatManager.firstMove.lastButtonSelected.Select();
            combatManager.SetState(combatManager.firstMove);
        }
    }

    public void TacticalOptionSelected(State state)
    {
        combatManager.SetState(state);
    }
}
