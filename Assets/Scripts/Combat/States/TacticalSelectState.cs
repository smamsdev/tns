using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalSelectState : State
{
    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, true);
        combatManager.combatMenuManager.tacticalMenuFirstButton = buttonSelected;
        combatManager.combatMenuManager.tacticalMenuFirstButton.Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.TacticalSelectMenu, false);
            combatManager.SetState(combatManager.firstMove);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.buttonSelected, Color.white);
        }
    }

    public void TacticalOptionSelected(State state)
    {
        combatManager.SetState(state);
    }
}
