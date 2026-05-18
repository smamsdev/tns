using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VictoryState : State
{
    public VictoryRewardsUI victoryRewardsUI;

    int XPEarned;
    public int partyMemberIndex = 0;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableAllMenus();
        victoryRewardsUI.DisplayMenu(true);
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);
        XPEarned = 0;
        partyMemberIndex = 0;
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
        yield return(Rewards());
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

    public IEnumerator Rewards()
    {
        XPEarned = CalculateXPEarned();

        if (XPEarned > 0)
            victoryRewardsUI.DisplayXPReward(XPEarned);

        TotalGearRewards();
        victoryRewardsUI.SizeUI();

        yield return victoryRewardsUI.AnimateRewardsPage(0, 1, .5f);
    }

    void TotalGearRewards()
    {
        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            var drop = enemy.ItemDrop();
            int i = 0;

            if (drop != null)
            {
                GearInstance dropInstance = new GearInstance();
                dropInstance.gearSO = drop;

                if (!combatManager.playerCombat.playerInventorySO.AttemptAddGearToInventory(dropInstance, true))
                    Debug.Log("no space rn you needc to build an inventory overflow system, have fun");

                victoryRewardsUI.DisplayGearReward(drop, i);
                i++;
            }
        }
    }

    public void CyclePartyMemberXPGain()
    {
        if (partyMemberIndex >= combatManager.allAlliesToTarget.Count)
        {
            StartCoroutine(EndBattle());
            return;
        }

        if (partyMemberIndex < combatManager.allAlliesToTarget.Count)
        {
            Combatant combatant = combatManager.allAlliesToTarget[partyMemberIndex];
            combatManager.cameraFollow.transformToFollow = combatManager.allAlliesToTarget[partyMemberIndex].transform;

            victoryRewardsUI.DisplayPartyMemberStats(combatant, XPEarned);

            partyMemberIndex++;
        }
    }

    public int CalculateXPEarned()
    {
        int XPEarned = 0;

        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            XPEarned += enemy.XPReward;
            if (enemy.XPReward == 0)
            {
                Debug.Log("no xp assigned for " + enemy.combatantName);
            }
        }

        return XPEarned;
    }
}
