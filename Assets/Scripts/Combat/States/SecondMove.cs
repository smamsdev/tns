using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManagerV3.combatUIScript.ShowSecondMoveMenu();
        combatManagerV3.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void StateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
          combatManagerV3.SetState(combatManagerV3.firstMove);
          CombatEvents.InputCoolDown?.Invoke(0.1f);
        }
    }
}
