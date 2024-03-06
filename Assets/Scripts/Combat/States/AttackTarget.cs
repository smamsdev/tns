using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManagerV3.combatUIScript.secondMoveMenu.SetActive(false);

        if (combatManagerV3.playerMoveManager.firstMoveIs == 1 || combatManagerV3.playerMoveManager.secondMoveIs == 1)

        {
            combatManagerV3.attackTargetMenuScript.DisplayAttackTargetMenu();
        }

        else { combatManagerV3.SetState(combatManagerV3.applyMove); }

        yield break;
    }

    public override void StateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManagerV3.SetState(combatManagerV3.secondMove);
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

}

