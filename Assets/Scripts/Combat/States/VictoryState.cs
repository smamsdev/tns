using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class VictoryState : State
{
    public VictoryRewardsUI victoryRewardsUI;

    int XPEarned;
    public int partyMemberIndex = 0;
    public List<GearSO> gearDrops = new();

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisableAllMenus();
        victoryRewardsUI.DisplayMenu(true);
        combatManager.playerCombat.combatantUI.statsDisplay.ShowStatsDisplay(false);
        XPEarned = 0;
        partyMemberIndex = 0;
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
        Rewards();

        yield return null;
    }

    public void Rewards()
    {
        XPEarned = CalculateXPEarned();

        if (XPEarned > 0)
            victoryRewardsUI.InstantiateXPRewardTextElement(XPEarned);

        TotalGearDrops();

        victoryRewardsUI.DisplayAllRewards();
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

    void TotalGearDrops()
    {
        int i = 0;
        gearDrops.Clear();

        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            GearSO drop = enemy.ItemDrop();

            if (drop == null)
                continue;

            gearDrops.Add(drop);
            victoryRewardsUI.InstantiateGearDropTextElement(drop, i);
            i++;
        }
    }

    public void CycleRewardDistributionButtonSelected()
    {
        if (partyMemberIndex >= combatManager.allAlliesToTarget.Count)
        {
            StartCoroutine(EndBattle());
            return;
        }

        else
            StartCoroutine(DistributeXPToPartyMember());
    }

    public IEnumerator DistributeXPToPartyMember()
    {
        PartyMemberCombat partyMember = combatManager.allAlliesToTarget[partyMemberIndex] as PartyMemberCombat;
        bool isPlayer = partyMember is PlayerCombat;

        combatManager.cameraFollow.transformToFollow = combatManager.allAlliesToTarget[partyMemberIndex].transform;

        //transition the menu down first if its already up
        if (partyMemberIndex > 0)
            yield return victoryRewardsUI.TransitionDistributionPageDown();

        int allyStartXP = partyMember.partyMemberPermanentStats.XP;
        int allyFinalXP = allyStartXP + XPEarned;

        partyMember.partyMemberPermanentStats.XP = allyFinalXP;

        victoryRewardsUI.UpdatePartyMemberStatsText(partyMember);
        victoryRewardsUI.DisplayDistributeUI(isPlayer);

        StartCoroutine(FieldEvents.LerpValuesCoRo(allyStartXP, allyFinalXP, 2, value =>
        {
            partyMember.partyMemberPermanentStats.XP = Mathf.RoundToInt(value);
            victoryRewardsUI.UpdatePartyMemberStatsText(partyMember);

            if (partyMember.partyMemberPermanentStats.XP >= partyMember.partyMemberPermanentStats.XPThreshold)
                HandleLevelUP(partyMember);
        }));

        partyMemberIndex++;
    }

    void HandleLevelUP(PartyMemberCombat partyMember)
    {
        partyMember.partyMemberPermanentStats.LevelUp();
        partyMember.partyMemberPermanentStats.UpdateThreshold();
        victoryRewardsUI.UpdatePartyMemberStatsText(partyMember);
        victoryRewardsUI.SizeGLGWidth(victoryRewardsUI.distributeXPTextElements, victoryRewardsUI.distributeGridLayoutGroup);
    }

    public IEnumerator EndBattle()
    {
        victoryRewardsUI.rewardsListContainerAnimator.Play("CloseMenu");
        yield return victoryRewardsUI.TransitionDistributionPageDown();
    
        combatManager.cameraFollow.transformToFollow = combatManager.playerCombat.transform;
    
        CombatEvents.isBattleMode = false;
        var playerCombat = combatManager.playerCombat;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        playerAnimator.SetFloat("lookDirectionX", combatManager.playerCombat.CombatLookDirX);
    
        if (combatManager.battleScheme.isRandomEnounter)
        {
            Debug.Log("do some scene entry/exit stuff here i dunno dude");
        }
    
        else
            CombatEvents.UnlockPlayerMovement();
    }

 //  public void DisplayPartyMemberStats(Combatant combatantInPlay, int startXP, int finalXP)
 //  {
 //      if (!XPRewardsDistributeParent.activeSelf)
 //          XPRewardsDistributeParent.SetActive(true);
 //
 //      distributionContainerAnimator.Play("OpenMenu");
 //
 //      if (combatantInPlay is PlayerCombat playerCombat)
 //      {
 //          foreach (GameObject go in playerStatsOnly)
 //          {
 //              go.SetActive(true);
 //          }
 //
 //          PlayerPermanentStats playerStats = playerCombat.playerPermanentStats;
 //
 //          playerFocusTMP.text = playerStats.FocusBase.ToString();
 //          playerStats.UpdateThreshold();
 //          allyNameTMP.text = combatantInPlay.combatantName;
 //          allyLevelTMP.text = playerStats.Level.ToString();
 //          allyAttackTMP.text = playerStats.AttackBase.ToString();
 //          allyFendTMP.text = playerStats.FendBase.ToString();
 //
 //          SizeGLGWidth(distributeXPTextElements, distributeGridLayoutGroup);
 //
 //          StartCoroutine(FieldEvents.LerpValuesCoRo(startXP, finalXP, 2, value =>
 //          {
 //              playerStats.XP = Mathf.RoundToInt(value);
 //              allyXPTMP.text = playerStats.XP.ToString();
 //              allyXPRemainderTMP.text = (playerStats.XPThreshold - playerStats.XP).ToString();
 //
 //              SizeGLGWidth(distributeXPTextElements, distributeGridLayoutGroup);
 //
 //              if (playerStats.XP >= playerStats.XPThreshold)
 //              {
 //                  playerStats.LevelUp();
 //                  allyLevelTMP.text = playerStats.Level.ToString();
 //                  allyAttackTMP.text = playerStats.AttackBase.ToString();
 //                  allyFendTMP.text = playerStats.FendBase.ToString();
 //                  playerFocusTMP.text = playerStats.FocusBase.ToString();
 //              }
 //          }));
 //      }
 //
 //      else
 //
 //      {
 //          PartyMemberCombat partyMemberCombat = combatantInPlay as PartyMemberCombat;
 //
 //          foreach (GameObject go in playerStatsOnly)
 //          { go.SetActive(false); }
 //
 //          PartyMemberPermanentStats PartyMemberPermanentStats = partyMemberCombat.partyMemberPermanentStats;
 //
 //          PartyMemberPermanentStats.UpdateThreshold();
 //          allyNameTMP.text = combatantInPlay.combatantName;
 //          allyLevelTMP.text = PartyMemberPermanentStats.Level.ToString();
 //          allyAttackTMP.text = PartyMemberPermanentStats.AttackBase.ToString();
 //          allyFendTMP.text = PartyMemberPermanentStats.FendBase.ToString();
 //
 //          SizeGLGWidth(distributeXPTextElements, distributeGridLayoutGroup);
 //
 //          StartCoroutine(FieldEvents.LerpValuesCoRo(startXP, finalXP, 2, value =>
 //          {
 //              PartyMemberPermanentStats.XP = Mathf.RoundToInt(value);
 //              allyXPTMP.text = PartyMemberPermanentStats.XP.ToString();
 //              allyXPRemainderTMP.text = (PartyMemberPermanentStats.XPThreshold - PartyMemberPermanentStats.XP).ToString();
 //
 //              SizeGLGWidth(distributeXPTextElements, distributeGridLayoutGroup);
 //
 //              if (PartyMemberPermanentStats.XP >= PartyMemberPermanentStats.XPThreshold)
 //              {
 //                  PartyMemberPermanentStats.LevelUp();
 //                  allyLevelTMP.text = PartyMemberPermanentStats.Level.ToString();
 //                  allyAttackTMP.text = PartyMemberPermanentStats.AttackBase.ToString();
 //                  allyFendTMP.text = PartyMemberPermanentStats.FendBase.ToString();
 //              }
 //          }));
 //      }
 //  }
 //
 //
}
