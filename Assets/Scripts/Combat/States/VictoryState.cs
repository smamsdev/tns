using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VictoryState : State
{
    public VictoryRewardsUI victoryRewardsUI;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableAllMenus();
        victoryRewardsUI.DisplayMenu(true);
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);

        yield return(victoryRewardsUI.ShowRewards());
        victoryRewardsUI.totalXPButton.Select();

        yield return null;
    }

    public IEnumerator EndBattle()
    {
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;

        CombatEvents.isBattleMode = false;
        var playerCombat = combatManager.playerCombat;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        playerAnimator.SetFloat("lookDirectionX", combatManager.playerCombat.CombatLookDirX);
        yield return (victoryRewardsUI.AnimateRewardsPage(1, 0, .5f));

        if (combatManager.battleScheme.isRandomEnounter)
        {
            Debug.Log("do some scene entry/exit stuff here i dunno dude");
        }

        else
        CombatEvents.UnlockPlayerMovement();
    }
}
