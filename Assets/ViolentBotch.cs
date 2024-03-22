using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolentBotch : ViolentMove
{
    public int hpLossMultiplier;
    public PlayerCombatStats playerCombatStats;

    public override void OnApplyMove()
    {
        StartCoroutine(OnApplyMoveCoro());
    }

    IEnumerator OnApplyMoveCoro()

    {

        yield return new WaitForSeconds(0.5f);
        CombatEvents.UpdatePlayerHP.Invoke(Mathf.RoundToInt(-playerCombatStats.playerMaxHP * 0.11f));
        CombatEvents.PlayerDamageDisplay.Invoke(Mathf.RoundToInt(-playerCombatStats.playerMaxHP * 0.11f));

        yield return new WaitForSeconds(1.0f);

        CombatEvents.DisablePlayerDamageDisplay();
        CombatEvents.EndMove.Invoke();
    }

    public override void OnEnemyAttack()

    {
        //blank
    }
}
