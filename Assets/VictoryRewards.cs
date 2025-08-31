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
    public TextMeshProUGUI[] totalXPTextElements;
    public TextMeshProUGUI[] rewardXPTextElements;
    public TextMeshProUGUI totalXPEarnedTMP;
    public GridLayoutGroup XPGainGridLayoutGroup;

    int XPEarned;
    public Button totalXPButton;
    public int partyToLoop = 0;

    public TextMeshProUGUI allyNameTMP, allyLevelTMP, allyXPRemainderTMP, allyXPTMP, allyAttackTMP, allyFendTMP, playerFocusTMP;

    public GameObject[] playerStatsOnly;

    float lastRewardWidth = 0f;

    TextMeshProUGUI FindLongestText(TextMeshProUGUI[] arrayToSort)
    {
        return arrayToSort.OrderByDescending(text => text.preferredWidth).First();
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
        totalXPEarnedTMP.text = XPEarned + " Experience gained";

        float preferredWidth = FindLongestText(totalXPTextElements).preferredWidth;
        Vector2 newCellSize = totalXPgridLayout.cellSize;
        newCellSize.x = preferredWidth;
        totalXPgridLayout.cellSize = newCellSize;

        totalXPButton.Select();
    }

    void UpdateXPGainLayout()
    {
        float preferredWidth = FindLongestText(rewardXPTextElements).preferredWidth;
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
}
