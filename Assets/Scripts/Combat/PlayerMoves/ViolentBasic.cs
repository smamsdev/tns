using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBasic : ViolentMove
{
    public bool isCounterAttack;

    public override void OnApplyMove()

    {
        if (!isCounterAttack)
        {
            CombatEvents.MeleeAttack?.Invoke();
        }

        else CombatEvents.EndMove?.Invoke();
    }

    public override void OnEnemyAttack()

    {
        if (isCounterAttack)
        {   
            CombatEvents.CounterAttack(); }

        
        else { return; }

    }
}
