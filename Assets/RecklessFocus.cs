using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecklessFocus : ViolentMove
{
    public override IEnumerator OnApplyMove(CombatManager _combatManager, Enemy _enemy)

    {
        combatManager = _combatManager;

        yield return new WaitForSeconds(0.5f);

        int damageToPlayer = Mathf.RoundToInt(-combatManager.playerCombat.playerMaxHP / 10);

        //CombatEvents.UpdatePlayerHP.Invoke(damageToPlayer);
        //CombatEvents.PlayerDamageDisplay.Invoke(damageToPlayer);
        yield return new WaitForSeconds(1);
    }

    public override IEnumerator OnEnemyAttack(CombatManager _combatManager, Enemy _enemy)

    {
        yield break;
    }

    public override IEnumerator Return()
    {
        Debug.Log("todo");
        yield return null;
    }
}
