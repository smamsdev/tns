using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject firstMoveContainer;

    private void OnEnable()
    {
        CombatEvents.SendMove += SetFirstMove;
    }

    private void OnDisable()
    {
        CombatEvents.SendMove -= SetFirstMove;
    }

    public override IEnumerator StartState()
    {
        if (combatManager == null)
        {
            Debug.LogError("CombatManager is null in StartState!");
            yield break;
        }

        combatManager.CombatUIManager.playerFendScript.animator.SetBool("fendbreak", false);

        yield return new WaitForSeconds(0.1f);

        combatManager.CombatUIManager.ShowFirstMoveMenu(true);
        combatManager.playerMoveManager.firstMoveIs = 0;

        yield break;
    }

    void SetFirstMove(int moveValue)
    {
        if (combatManager == null)
        {
            Debug.LogError("CombatManager is null in SetFirstMove!");
            return;
        }

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
        }

        if (moveValue == 4)
        {
            Debug.Log(combatManager);
            combatManager.SetState(combatManager.gearSelect);
        }
        else
        {
            combatManager.SetState(combatManager.secondMove);
        }

        combatManager.CombatUIManager.UpdateFirstMoveDisplay(moveName);
    }

    public void FirstMoveIsViolent()
    {
        combatManager.playerMoveManager.firstMoveIs = 1;
        combatManager.CombatUIManager.UpdateFirstMoveDisplay("Violent");
        combatManager.SetState(combatManager.secondMove);
    }
}