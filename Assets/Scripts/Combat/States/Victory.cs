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
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.VictoryMenu, true);
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);

        yield return(victoryRewards.ShowRewards());
        victoryRewards.totalXPButton.Select();

        yield return null;
    }

    public IEnumerator EndBattle()
    {
        yield return (victoryRewards.AnimateRewardsPage(1, 0, .5f));

        CombatEvents.isBattleMode = true;
        var playerCombat = combatManager.playerCombat;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", false);
        playerAnimator.Play("Idle");
        combatManager.cameraFollow.transformToFollow = combatManager.player.transform;
        CombatEvents.UnlockPlayerMovement();

    }
}
