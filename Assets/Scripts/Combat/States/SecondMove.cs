using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.ShowSecondMoveMenu();
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
}
