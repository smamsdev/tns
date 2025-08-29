using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Victory : State
{
    public VictoryRewards victoryRewards;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableMenuState();
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);

        victoryRewards.ShowTotalXP();

        yield return null;
    }
}
