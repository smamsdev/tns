using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VictoryRewards : MonoBehaviour
{
    public CombatManager combatManager;
    public GridLayoutGroup totalXPgridLayout;
    public List<TextMeshProUGUI> rewardTextElements, distributeXPTextElements;
    public GridLayoutGroup XPGainGridLayoutGroup;

    int XPEarned;
    public Button totalXPButton;
    public int partyToLoop = 0;

    public TextMeshProUGUI allyNameTMP, allyLevelTMP, allyXPRemainderTMP, allyXPTMP, allyAttackTMP, allyFendTMP, playerFocusTMP;

    public GameObject[] playerStatsOnly;
    public TextMeshProUGUI[] defaultRewardTextElements;

    public GameObject uiRewardSlotPrefab, rewardsParent;

    float lastRewardWidth = 0f;

    TextMeshProUGUI FindLongestText(List<TextMeshProUGUI> textElementsToSort)
    {
        return textElementsToSort.OrderByDescending(text => text.preferredWidth).First();
    }

    void TotalXPEarned()
    {
        XPEarned = 0;
        foreach (Enemy enemy in combatManager.enemies)
        {
            XPEarned += enemy.XPReward;
            if (enemy.XPReward == 0)
            {
                Debug.Log("no xp assigned for " + enemy.combatantName);
            }
        }
    }

    public void ShowRewards()
    {
        partyToLoop = 0;
        TotalXPEarned();

        rewardTextElements = new List<TextMeshProUGUI>();
        rewardTextElements.Add(defaultRewardTextElements[0]);
        rewardTextElements.Add(defaultRewardTextElements[1]);

        ShowXPReward();
        ShowItemReward();


        float preferredWidth = FindLongestText(rewardTextElements).preferredWidth;
        Vector2 newCellSize = totalXPgridLayout.cellSize;
        newCellSize.x = preferredWidth;
        totalXPgridLayout.cellSize = newCellSize;

        totalXPButton.Select();
    }

    void UpdateXPGainLayout()
    {
        float preferredWidth = FindLongestText(distributeXPTextElements).preferredWidth;
        if (Mathf.Approximately(preferredWidth, lastRewardWidth)) return;
        lastRewardWidth = preferredWidth;

        Vector2 newCellSize = XPGainGridLayoutGroup.cellSize;
        Vector2 newSpacing = XPGainGridLayoutGroup.spacing;

        newCellSize.x = 50 + (preferredWidth - 50);
        newSpacing.x = 150 - (preferredWidth - 50);

        XPGainGridLayoutGroup.cellSize = newCellSize;
        XPGainGridLayoutGroup.spacing = newSpacing;
    }

    public void CyclePartyMemberXPGain()
    {
        if (partyToLoop < combatManager.allAlliesToTarget.Count)
        {
            if (!combatManager.allAlliesToTarget[partyToLoop].isEncounterSpawned)
            {
                Ally ally = combatManager.allAlliesToTarget[partyToLoop] as Ally;
                AllyPermanentStats stats;

                if (ally is PlayerCombat playerCombat)
                {
                    foreach (GameObject go in playerStatsOnly)
                    { go.SetActive(true); }

                    PlayerPermanentStats playerStats = playerCombat.playerPermanentStats;
                    stats = playerStats;
                    playerFocusTMP.text = playerStats.focusBase.ToString();
                }
                else
                {
                    foreach (GameObject go in playerStatsOnly)
                    { go.SetActive(false); }
                    stats = ally.allyPermanentStats;
                }

                stats.UpdateThreshold();
                allyNameTMP.text = ally.combatantName;
                allyLevelTMP.text = stats.level.ToString();
                allyAttackTMP.text = stats.attackBase.ToString();
                allyFendTMP.text = stats.fendBase.ToString();

                var previousXP = stats.XP;
                var targetXP = previousXP + XPEarned;
                allyXPTMP.text = previousXP.ToString();

                UpdateXPGainLayout();

                FieldEvents.LerpValues(previousXP, targetXP, 1, value =>
                {
                    stats.XP = value;
                    allyXPTMP.text = value.ToString();
                    allyXPRemainderTMP.text = (stats.XPThreshold - stats.XP).ToString();

                    UpdateXPGainLayout();

                    if (stats.XP >= stats.XPThreshold)
                    {
                        stats.LevelUp();
                        allyLevelTMP.text = stats.level.ToString();
                        allyAttackTMP.text = stats.attackBase.ToString();
                        allyFendTMP.text = stats.fendBase.ToString();

                        if (stats is PlayerPermanentStats playerStats)
                        {
                            playerFocusTMP.text = playerStats.focusBase.ToString();
                        }
                    }
                });

                combatManager.cameraFollow.transformToFollow = combatManager.allAlliesToTarget[partyToLoop].transform;
            }

            partyToLoop++;
        }
    }

    void ShowXPReward()
    {
        if (XPEarned > 0)
        {
            GameObject rewardXPSlotGO = Instantiate(uiRewardSlotPrefab);
            rewardXPSlotGO.transform.SetParent(rewardsParent.transform);
            rewardXPSlotGO.name = "XPEarned";
            var rewardSlotTMP = rewardXPSlotGO.GetComponent<TextMeshProUGUI>();
            rewardSlotTMP.text = XPEarned + " Experience";

            rewardTextElements.Add(rewardSlotTMP);
        }
    }

    void ShowItemReward()
    {
        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            var drop = enemy.ItemDrop();
            int i = 0;

            if (drop != null)
            { 
                combatManager.playerCombat.playerInventory.AddGearToInventory(drop);
                i++;
                GameObject rewardItemSlotGO = Instantiate(uiRewardSlotPrefab);
                rewardItemSlotGO.transform.SetParent(rewardsParent.transform);
                rewardItemSlotGO.name = "ItemDrop" + i;
                var rewardSlotTMP = rewardItemSlotGO.GetComponent<TextMeshProUGUI>();
                rewardSlotTMP.text = drop.gearName;
                rewardTextElements.Add(rewardSlotTMP);
            }
        }
    }
}
