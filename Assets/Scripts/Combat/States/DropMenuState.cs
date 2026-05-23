using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropMenuState : State
{
    public DropMenuManager dropMenuManager;

    public override IEnumerator StartState()
    {
        dropMenuManager.gameObject.SetActive(true);
        dropMenuManager.OpenDropMenu();
        yield return null;
    }

    public void ExitState()
    {
        dropMenuManager.dropMainMenu.ExitMenu();
        dropMenuManager.gameObject.SetActive(false);
        combatManager.currentState = combatManager.victoryState;
        combatManager.victoryState.EndBattle();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitState();
        }
    }
}
