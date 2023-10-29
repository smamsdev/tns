using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTarget : State
{

    public AttackTarget(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)
    {
    }

    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        if (combatManagerV3.playerMoveManager.firstMoveIs == 1 || combatManagerV3.playerMoveManager.secondMoveIs == 1)

        {
            combatManagerV3.attackTargetMenuScript.DisplayAttackTargetMenu();
        }

        else { combatManagerV3.SetBattleStateApplyPlayerMove(); }

        yield break;
    }

    public override void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManagerV3.SetBattleStateSecondMove();
            CombatEvents.InputCoolDown?.Invoke(0.2f);
        }
    }

}

