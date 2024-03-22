using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CautiousBasic : CautiousMove
{
    public bool isAttack;

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
