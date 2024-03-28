using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        CombatEvents.SendMove += SetSecondMove;

        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.ShowSecondMoveMenu(true);
        combatManager.combatUIScript.ShowFirstMoveMenu(false);
        combatManager.combatUIScript.ShowEnemySelectMenu(false);
        combatManager.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
          combatManager.SetState(combatManager.firstMove);
          CombatEvents.InputCoolDown?.Invoke(0.1f);
        }
    }

    void SetSecondMove(int moveValue)
    {
        combatManager.playerMoveManager.secondMoveIs = moveValue;
        string moveName = "";

        switch (moveValue)

        {
            case 1:
                moveName = "Attack";
                break;
            case 2:
                moveName = "Fend";
                break;
            case 3:
                moveName = "Focus";
                break;
        };

        combatManager.playerMoveManager.CombineStanceAndMove();
        combatManager.selectedPlayerMove = combatManager.playerMoveManager.GetSelectedPlayerMove();

        if (combatManager.selectedPlayerMove.isAttack)
        { combatManager.SetState(combatManager.enemySelect);
        }

        else
        { combatManager.SetState(combatManager.applyMove); }

        combatManager.combatUIScript.UpdateSecondMoveDisplay(moveName);
        CombatEvents.SendMove -= SetSecondMove;
    }

    private void OnDisable()
    {
        CombatEvents.SendMove -= SetSecondMove;
    }
}
