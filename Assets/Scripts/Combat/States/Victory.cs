using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Victory : State
{
    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableMenuState();
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);

        yield return new WaitForSeconds(0);

        Debug.Log("test");

        //FieldEvents.UpdateXP(combatManager.battleScheme.enemies[combatManager.selectedEnemy]);
    }
}
