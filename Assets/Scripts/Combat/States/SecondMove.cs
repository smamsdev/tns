using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondMove : State
{

    public SecondMove(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)
    {
    }

    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        combatManagerV3.combatUIScript.ShowSecondMoveMenu();
        combatManagerV3.playerMoveManager.secondMoveIs = 0;

        yield break;
    }

    public override void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))

        {
            combatManagerV3.SetBattleStateFirstMove();
            CombatEvents.InputCoolDown?.Invoke(0.1f);
        }
    }
}
