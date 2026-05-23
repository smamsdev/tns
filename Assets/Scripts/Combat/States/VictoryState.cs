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
        combatManager.playerCombat.combatantUI.combatUIContainer.SetActive(false);
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
            AddDropsToInventory();
            return;
        }

        else
            StartCoroutine(DistributeXPToPartyMember());
    }

    void AddDropsToInventory()
    {
        PlayerInventorySO playerInventorySO = combatManager.playerCombat.playerInventorySO;

        //reverse forloop because we are modifying the list as we go
        for (int i = gearDrops.Count - 1; i >= 0; i--)
        {
            GearSO gearSO = gearDrops[i];

            bool spaceAvailable =
                playerInventorySO.AttemptAddGearToInventory(
                    gearSO.CreateInstance(),
                    true);

            if (spaceAvailable)
            {
                gearDrops.RemoveAt(i);
            }
            else
            {
                combatManager.dropMenuState.dropMenuManager.dropMainMenu.rawDropList = gearDrops;
                combatManager.SetState(combatManager.dropMenuState);
                return;
            }
        }

        StartCoroutine(EndBattle());
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
        WritePartyMemberPermanantStats();
    
        var playerCombat = combatManager.playerCombat;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        playerAnimator.SetFloat("lookDirectionX", combatManager.playerCombat.CombatLookDirX);
        playerCombat.movementScript.rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
        playerCombat.collisionCollider.enabled = true;
        combatManager.cameraFollow.transformToFollow = playerCombat.transform;

        if (combatManager.battleScheme.isRandomEnounter)
        {
            CombatEvents.UnlockPlayerMovement();
            Debug.Log("do some scene entry/exit stuff here i dunno dude");
            yield break;
        }

        if (combatManager.battleScheme.isSpawningPartyMembers)
        {
            foreach (PartyMemberCombat partyMember in combatManager.allies)
            {
                partyMember.movementScript.animator.Play("Idle");
                yield return combatManager.PositionCombatant(partyMember.gameObject, playerCombat.transform.position);
                GameObject.Destroy(partyMember.gameObject);
                yield return new WaitForSeconds(0.25f);
            }

            CombatEvents.UnlockPlayerMovement();
        }


        else
            CombatEvents.UnlockPlayerMovement();
    }


    void WritePartyMemberPermanantStats()
    {
        foreach (PartyMemberCombat partyMember in combatManager.allAlliesToTarget)
        {
            partyMember.partyMemberPermanentStats.CurrentHP = partyMember.CurrentHP;

            if (partyMember is PlayerCombat playerCombat)
                playerCombat.playerPermanentStats.CurrentPotential = playerCombat.CurrentPotential; 
        }
    }
}
