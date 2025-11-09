using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VictoryState : State
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
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;

        CombatEvents.isBattleMode = false;
        var playerCombat = combatManager.playerCombat;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        playerAnimator.SetFloat("lookDirectionX", combatManager.playerCombat.CombatLookDirX);
        yield return (victoryRewards.AnimateRewardsPage(1, 0, .5f));

        if (combatManager.battleScheme.isRandomEnounter)
        {
            FieldEvents.isReturningFromEncounter = true;
            SceneManager.LoadScene(FieldEvents.sceneBeforeEncounterName);
        }

        else
        CombatEvents.UnlockPlayerMovement();
    }
}
