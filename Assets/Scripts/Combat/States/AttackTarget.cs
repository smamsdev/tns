using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {

        combatManager.playerMoveManager.CombineStanceAndMove();
        combatManager.selectedPlayerMove = combatManager.playerMoveManager.GetSelectedPlayerMove();

        yield return new WaitForSeconds(0.1f);

        combatManager.combatUIScript.secondMoveMenu.SetActive(false);

        if (combatManager.selectedPlayerMove.isAttack)

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

