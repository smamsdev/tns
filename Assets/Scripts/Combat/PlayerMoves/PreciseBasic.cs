using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreciseBasic : PreciseMove
{

    public override void OnApplyMove()

    {
            CombatEvents.MeleeAttack();
    }

    public override void OnEnemyAttack()

    {
        //blank
    }

}
