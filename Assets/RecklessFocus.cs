using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecklessFocus : ViolentMove
{
    [SerializeField] PlayerCombatStats playerCombatStats;

    public override void OnApplyMove()

    {
        CombatEvents.UpdatePlayerHP.Invoke(Mathf.RoundToInt(-playerCombatStats.playerMaxHP /10));
        CombatEvents.EndMove.Invoke();
    }

    public override void OnEnemyAttack()

    {
        //blank
    }

}
