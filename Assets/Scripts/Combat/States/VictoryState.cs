using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class VictoryState : State
{
    public VictoryRewardsUI victoryRewardsUI;
    int XPEarned;
    public int partyMemberIndex = 0;
    public List<GearSO> rawGearSODrops = new();
    public InventorySO finalGearDropList;

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
        InstantiateDropListSOs(rawGearSODrops);
        victoryRewardsUI.InstantiateGearDropTextElement(finalGearDropList.gearInstanceInventory);
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
        rawGearSODrops.Clear();

        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            GearSO drop = enemy.ItemDrop();

            if (drop == null)
                continue;

            rawGearSODrops.Add(drop);
            i++;
        }
    }

    public void CycleRewardDistributionButtonSelected()
    {
        if (partyMemberIndex >= combatManager.allAlliesToTarget.Count)
        {
            StartCoroutine(AddDropsToInventory());
            return;
        }

        else
            StartCoroutine(DistributeXPToPartyMember());
    }

    void InstantiateDropListSOs(List<GearSO> dropSOList)
    {
        finalGearDropList.gearInstanceInventory.Clear();
        rawGearSODrops.ShuffleList();

        //init 5 max empty slots
        for (int i = 0; i < 5; i++)
        {
            var emptyInstance = new GearInstance();
            finalGearDropList.gearInstanceInventory.Add(emptyInstance);
        }

        //drop list should be limited to the first 4 items max
        for (int i = 0; i < Mathf.Min(dropSOList.Count, 4); i++)
        {
            if (dropSOList[i] == null)
                continue;

            var gearInstance = dropSOList[i].CreateInstance();

            if (gearInstance is EquipmentInstance equipmentInstance)
            {
                int randomValue = Random.Range(0, equipmentInstance.MaxPotential() / 2);
                equipmentInstance.SetCharge(randomValue);
            }

            if (!finalGearDropList.AttemptAddGearToInventory(gearInstance, true))
                return;
        }
    }

    IEnumerator AddDropsToInventory()
    {
        PlayerInventorySO playerInventorySO = combatManager.playerCombat.playerInventorySO;

        for (int i = 0; i < finalGearDropList.gearInstanceInventory.Count; i++)
        {
            var gearInstance = finalGearDropList.gearInstanceInventory[i];

            if (gearInstance.gearSO == null)
                continue;

            bool spaceAvailable =playerInventorySO.AttemptAddGearToInventory(gearInstance, true);

            if (spaceAvailable)
            {
                //repeat this for consumable stacks
                finalGearDropList.RemoveGearFromInventory(gearInstance, true);
                i--;
                continue;
            }

            victoryRewardsUI.DisplayMenu(false);
            yield return new WaitForSeconds(0.25f);

            combatManager.dropMenuState.dropMenuManager.dropMainMenu.dropManagerInventorySO = finalGearDropList;
            combatManager.SetState(combatManager.dropMenuState);
            yield break;
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
        playerAnimator.CrossFade("Idle", 0.2f);
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
        }

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
