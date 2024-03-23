using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseBasic : PreciseMove
{

    public override void OnApplyMove()

    {
        if (isAttack)

            CombatEvents.MeleeAttack.Invoke();

        else

            CombatEvents.EndMove.Invoke();
    }

    public override void OnEnemyAttack()

    {
        //blank
    }

}
