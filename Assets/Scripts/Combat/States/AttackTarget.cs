using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.secondMoveMenu.SetActive(false);

        if (combatManager.playerMoveManager.firstMoveIs == 1 || combatManager.playerMoveManager.secondMoveIs == 1)

        {
            combatManager.attackTargetMenuScript.DisplayAttackTargetMenu();
        }

        else { combatManager.SetState(combatManager.applyMove); }

        yield break;
    }

    public override void StateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManager.SetState(combatManager.secondMove);
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

}

