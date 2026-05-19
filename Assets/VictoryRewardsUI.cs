using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class VictoryRewardsUI : MonoBehaviour
{
    public CombatManager combatManager;
    public GridLayoutGroup totalXPgridLayout;
    public List<TextMeshProUGUI> rewardTextElements, distributeXPTextElements;
    public GridLayoutGroup XPGainGridLayoutGroup;
    public VictoryState victoryState;

    public Button totalXPButton;
    public TextMeshProUGUI allyNameTMP, allyLevelTMP, allyXPRemainderTMP, allyXPTMP, allyAttackTMP, allyFendTMP, playerFocusTMP;
    public Sprite XPIcon;
    public GameObject[] playerStatsOnly;
    public TextMeshProUGUI[] defaultRewardTextElements;

    public GameObject uiRewardSlotPrefab, rewardsParent, XPRewardsDistributeParent;
    public Animator rewardsListContainerAnimator, distributionContainerAnimator;

    float lastRewardWidth = 0f;

    public void DisplayMenu(bool on)
    {
        this.gameObject.SetActive(on);
    }

    public IEnumerator AnimateRewardsPage(float start, float end, float duration)
    {
        var rewardsRect = this.transform as RectTransform;
        var scale = rewardsRect.localScale;

        rewardTextElements.Clear();
        rewardTextElements.Add(defaultRewardTextElements[0]);
        rewardTextElements.Add(defaultRewardTextElements[1]);
        XPRewardsDistributeParent.SetActive(false);

        FieldEvents.LerpValuesCoRo(start, end, duration, animateScale =>
        {
            rewardsRect.localScale = new Vector3(animateScale, animateScale, animateScale);
        });

        yield return new WaitForSeconds(duration);
    }

    public void SizeUI()
    {
        float preferredWidth = FieldEvents.FindLongestText(rewardTextElements).preferredWidth;
        Vector2 newCellSize = totalXPgridLayout.cellSize;
        newCellSize.x = preferredWidth;
        totalXPgridLayout.cellSize = newCellSize;
    }

    void UpdateXPGainLayout()
    {
        float preferredWidth = FieldEvents.FindLongestText(distributeXPTextElements).preferredWidth;
        if (Mathf.Approximately(preferredWidth, lastRewardWidth)) return;
        lastRewardWidth = preferredWidth;

        Vector2 newCellSize = XPGainGridLayoutGroup.cellSize;
        Vector2 newSpacing = XPGainGridLayoutGroup.spacing;

        newCellSize.x = 50 + (preferredWidth - 50);
        newSpacing.x = 150 - (preferredWidth - 50);

        XPGainGridLayoutGroup.cellSize = newCellSize;
        XPGainGridLayoutGroup.spacing = newSpacing;
    }

    public void DisplayXPReward(int XPEarned)
    {
        XPRewardsDistributeParent.SetActive(false);
        rewardsListContainerAnimator.Play("OpenMenu");

        rewardTextElements.Clear();
        rewardTextElements.Add(defaultRewardTextElements[0]);
        rewardTextElements.Add(defaultRewardTextElements[1]);

        GameObject rewardXPSlotGO = Instantiate(uiRewardSlotPrefab);
        rewardXPSlotGO.transform.SetParent(rewardsParent.transform);
        rewardXPSlotGO.name = "XPEarned";
        InventorySlotUI inventorySlotUI = rewardXPSlotGO.GetComponent<InventorySlotUI>();
        inventorySlotUI.itemNameTMP.text = XPEarned + " Experience";
        inventorySlotUI.icon.sprite = XPIcon;

        rewardTextElements.Add(inventorySlotUI.itemNameTMP);
    }

    public IEnumerator TransitionDistributionPageDown()
    {
        distributionContainerAnimator.Play("CloseMenu");
        yield return new WaitForSeconds(0.5f);
    }

    public void DisplayPartyMemberStats(Combatant combatantInPlay, int XPEarned)
    {
        if (!XPRewardsDistributeParent.activeSelf)
            XPRewardsDistributeParent.SetActive(true);

        distributionContainerAnimator.Play("OpenMenu");

        if (combatantInPlay is PlayerCombat playerCombat)
        {
            foreach (GameObject go in playerStatsOnly)
            {
                go.SetActive(true);
            }

            PlayerPermanentStats playerStats = playerCombat.playerPermanentStats;

            playerFocusTMP.text = playerStats.FocusBase.ToString();
            playerStats.UpdateThreshold();
            allyNameTMP.text = combatantInPlay.combatantName;
            allyLevelTMP.text = playerStats.Level.ToString();
            allyAttackTMP.text = playerStats.AttackBase.ToString();
            allyFendTMP.text = playerStats.FendBase.ToString();

            var previousXP = playerStats.XP;
            var targetXP = previousXP + XPEarned;
            allyXPTMP.text = previousXP.ToString();

            UpdateXPGainLayout();

            FieldEvents.LerpValuesCoRo(previousXP, targetXP, 1, value =>
            {
                playerStats.XP = Mathf.RoundToInt(value);
                allyXPTMP.text = playerStats.XP.ToString();
                allyXPRemainderTMP.text = (playerStats.XPThreshold - playerStats.XP).ToString();

                UpdateXPGainLayout();

                if (playerStats.XP >= playerStats.XPThreshold)
                {
                    playerStats.LevelUp();
                    allyLevelTMP.text = playerStats.Level.ToString();
                    allyAttackTMP.text = playerStats.AttackBase.ToString();
                    allyFendTMP.text = playerStats.FendBase.ToString();
                    playerFocusTMP.text = playerStats.FocusBase.ToString();
                }
            });
        }

        else

        {
            PartyMemberCombat partyMemberCombat = combatantInPlay as PartyMemberCombat;

            foreach (GameObject go in playerStatsOnly)
            { go.SetActive(false); }

            PartyMemberPermanentStats PartyMemberPermanentStats = partyMemberCombat.partyMemberPermanentStats;

            PartyMemberPermanentStats.UpdateThreshold();
            allyNameTMP.text = combatantInPlay.combatantName;
            allyLevelTMP.text = PartyMemberPermanentStats.Level.ToString();
            allyAttackTMP.text = PartyMemberPermanentStats.AttackBase.ToString();
            allyFendTMP.text = PartyMemberPermanentStats.FendBase.ToString();

            var previousXP = PartyMemberPermanentStats.XP;
            var targetXP = previousXP + XPEarned;
            allyXPTMP.text = previousXP.ToString();

            UpdateXPGainLayout();

            FieldEvents.LerpValuesCoRo(previousXP, targetXP, 1, value =>
            {
                PartyMemberPermanentStats.XP = Mathf.RoundToInt(value);
                allyXPTMP.text = PartyMemberPermanentStats.XP.ToString();
                allyXPRemainderTMP.text = (PartyMemberPermanentStats.XPThreshold - PartyMemberPermanentStats.XP).ToString();

                UpdateXPGainLayout();

                if (PartyMemberPermanentStats.XP >= PartyMemberPermanentStats.XPThreshold)
                {
                    PartyMemberPermanentStats.LevelUp();
                    allyLevelTMP.text = PartyMemberPermanentStats.Level.ToString();
                    allyAttackTMP.text = PartyMemberPermanentStats.AttackBase.ToString();
                    allyFendTMP.text = PartyMemberPermanentStats.FendBase.ToString();
                }
            });
        }

    }

    public void DisplayGearDropUI(GearSO drop, int i)
    {
        GameObject rewardGearSlotUIGO = Instantiate(uiRewardSlotPrefab);
        rewardGearSlotUIGO.transform.SetParent(rewardsParent.transform);
        rewardGearSlotUIGO.name = "ItemDrop" + (i + 1);
        InventorySlotUI inventorySlotUI = rewardGearSlotUIGO.GetComponent<InventorySlotUI>();
        inventorySlotUI.itemNameTMP.text = drop.GearName;
        rewardTextElements.Add(inventorySlotUI.itemNameTMP);

        bool isEquipment = drop is EquipmentSO;
        inventorySlotUI.icon.sprite = isEquipment? inventorySlotUI.equipmentIcon : inventorySlotUI.consumableIcon;
    }
}
