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
        combatManager.currentState = combatManager.victoryState;
        StartCoroutine(combatManager.victoryState.EndBattle());
    }

    public override void StateUpdate() { }// see dropMenuManager state
}
