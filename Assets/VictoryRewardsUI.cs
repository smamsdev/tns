using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryRewardsUI : MonoBehaviour
{
    public CombatManager combatManager;
    public GridLayoutGroup allRewardsGridLayoutGroup;
    public GridLayoutGroup distributeGridLayoutGroup;
    public List<TextMeshProUGUI> allRewardTextElements, distributeXPTextElements;
    public VictoryState victoryState;
    public Button allRewardsButton;
    public TextMeshProUGUI allyNameTMP, allyLevelTMP, allyXPRemainderTMP, allyXPTMP, allyHP, allyAttackTMP, allyFendTMP, playerPotentialTMP, playerFocusTMP;
    public Sprite XPIcon;
    public GameObject[] playerStatsOnly;
    public TextMeshProUGUI[] defaultRewardTextElements;

    public GameObject uiRewardSlotPrefab, rewardsListParent, XPRewardsDistributeParent;
    public Animator rewardsListContainerAnimator, distributionContainerAnimator;

    public void DisplayMenu(bool on)
    {
        if (on)
        {
            this.gameObject.SetActive(on);
            return;
        }
        
        if (!on && this.gameObject.activeSelf)
        {
            rewardsListContainerAnimator.Play("CloseMenu");
            distributionContainerAnimator.Play("CloseMenu");
        }
    }

    public void DisplayAllRewards()
    {
        rewardsListParent.SetActive(true);
        XPRewardsDistributeParent.SetActive(false);

        SizeGLGWidth(allRewardTextElements, allRewardsGridLayoutGroup);
        allRewardsButton.Select();
    }

    public void SizeGLGWidth(List<TextMeshProUGUI> tmpList, GridLayoutGroup glg)
    {
        float preferredWidth = FieldEvents.FindLongestText(tmpList).preferredWidth;
        Vector2 newCellSize = glg.cellSize;
        newCellSize.x = preferredWidth;
        glg.cellSize = newCellSize;
    }
    public void InstantiateXPRewardTextElement(int XPEarned)
    {
        rewardsListContainerAnimator.Play("OpenMenu");

        allRewardTextElements.Clear();

        GameObject rewardXPSlotGO = Instantiate(uiRewardSlotPrefab, rewardsListParent.transform);
        rewardXPSlotGO.name = "XPEarned";
        InventorySlotUI inventorySlotUI = rewardXPSlotGO.GetComponent<InventorySlotUI>();
        inventorySlotUI.itemNameTMP.text = XPEarned + " Experience";
        inventorySlotUI.icon.sprite = XPIcon;

        allRewardTextElements.Add(defaultRewardTextElements[0]);
        allRewardTextElements.Add(defaultRewardTextElements[1]);
        allRewardTextElements.Add(inventorySlotUI.itemNameTMP);
    }

    public void InstantiateGearDropTextElement(List <GearInstance> gearInstances)
    {
        foreach (GearInstance gearInstance in gearInstances)
        {
            if (gearInstance.gearSO == null)
                continue;

            bool isEquipment = gearInstance is EquipmentInstance;
            GameObject rewardGearSlotUIGO = Instantiate(uiRewardSlotPrefab, rewardsListParent.transform);

            rewardGearSlotUIGO.name = "GearDrop" + gearInstance.gearSO.name + gearInstance.QuantityString();

            InventorySlotUI inventorySlotUI = rewardGearSlotUIGO.GetComponent<InventorySlotUI>();

            inventorySlotUI.gearInstance = gearInstance;
            inventorySlotUI.itemNameTMP.text = inventorySlotUI.gearInstance.gearSO.GearName;
            inventorySlotUI.itemQuantityTMP.text = inventorySlotUI.gearInstance.QuantityString();
            inventorySlotUI.icon.sprite = isEquipment ? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;
            allRewardTextElements.Add(inventorySlotUI.itemNameTMP);
        }
    }

    public IEnumerator TransitionDistributionPageDown()
    {
        distributionContainerAnimator.Play("CloseMenu");
        yield return new WaitForSeconds(0.5f);
    }

    public void UpdatePartyMemberStatsText(PartyMemberCombat partyMemberCombat)
    {
        PartyMemberPermanentStats permanentStats = partyMemberCombat.partyMemberPermanentStats;

        allyNameTMP.text = partyMemberCombat.combatantName;
        allyLevelTMP.text = permanentStats.Level.ToString();
        allyXPTMP.text = permanentStats.XP.ToString();
        allyXPRemainderTMP.text = (permanentStats.XPThreshold - permanentStats.XP).ToString();
        allyHP.text = permanentStats.GetHPString();
        allyAttackTMP.text = permanentStats.AttackBase.ToString();
        allyFendTMP.text = permanentStats.FendBase.ToString();

        if (partyMemberCombat is PlayerCombat playerCombat)
        {
            playerPotentialTMP.text = playerCombat.playerPermanentStats.GetPotentialString();
            playerFocusTMP.text = playerCombat.playerPermanentStats.FocusBase.ToString();
        }


        else
            playerFocusTMP.text = "";
    }

    public void DisplayDistributeUI(bool isPlayer)
    {
        XPRewardsDistributeParent.SetActive(true);
        distributionContainerAnimator.Play("OpenMenu");
        SizeGLGWidth(distributeXPTextElements, distributeGridLayoutGroup);

        foreach (GameObject go in playerStatsOnly)
        {
            go.SetActive(isPlayer);
        }
    }
}
