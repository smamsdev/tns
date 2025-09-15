using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class VictoryRewards : MonoBehaviour
{
    public CombatManager combatManager;
    public GridLayoutGroup totalXPgridLayout;
    public List<TextMeshProUGUI> rewardTextElements, distributeXPTextElements;
    public GridLayoutGroup XPGainGridLayoutGroup;
    public VictoryState victory;

    int XPEarned;
    public Button totalXPButton;
    public int partyToLoop = 0;

    public TextMeshProUGUI allyNameTMP, allyLevelTMP, allyXPRemainderTMP, allyXPTMP, allyAttackTMP, allyFendTMP, playerFocusTMP;

    public GameObject[] playerStatsOnly;
    public TextMeshProUGUI[] defaultRewardTextElements;

    public GameObject uiRewardSlotPrefab, rewardsParent, XPRewardsDistributeParent;

    float lastRewardWidth = 0f;

    TextMeshProUGUI FindLongestText(List<TextMeshProUGUI> textElementsToSort)
    {
        return textElementsToSort.OrderByDescending(text => text.preferredWidth).First();
    }

    void TotalXPEarned()
    {
        XPEarned = 0;
        foreach (Enemy enemy in combatManager.battleScheme.enemies)
        {
            XPEarned += enemy.XPReward;
            if (enemy.XPReward == 0)
            {
                Debug.Log("no xp assigned for " + enemy.combatantName);
            }
        }
    }

    public IEnumerator ShowRewards()
    {
        XPRewardsDistributeParent.SetActive(false);
        
        partyToLoop = 0;
        rewardTextElements.Clear();
        rewardTextElements.Add(defaultRewardTextElements[0]);
        rewardTextElements.Add(defaultRewardTextElements[1]);
        
        TotalXPEarned();
        ShowXPReward();
        ShowItemReward();
        
        float preferredWidth = FindLongestText(rewardTextElements).preferredWidth;
        Vector2 newCellSize = totalXPgridLayout.cellSize;
        newCellSize.x = preferredWidth;
        totalXPgridLayout.cellSize = newCellSize;

        yield return AnimateRewardsPage(0, 1, .5f);
    }

    public IEnumerator AnimateRewardsPage(float start, float end, float duration)
    {
        var rewardsRect = this.transform as RectTransform;
        var scale = rewardsRect.localScale;

        FieldEvents.LerpValues(start, end, duration, animateScale =>
        {
            rewardsRect.localScale = new Vector3(animateScale, animateScale, animateScale);
        });

        yield return new WaitForSeconds(duration);
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
        if (!XPRewardsDistributeParent.activeSelf) { XPRewardsDistributeParent.SetActive(true); }
        
        if (partyToLoop >= combatManager.allAlliesToTarget.Count)
        {
            StartCoroutine(victory.EndBattle());
            return;
        }
                
        if (partyToLoop < combatManager.allAlliesToTarget.Count)
        {
            Combatant combatant = combatManager.allAlliesToTarget[partyToLoop];
            combatManager.cameraFollow.transformToFollow = combatManager.allAlliesToTarget[partyToLoop].transform;

            if (combatant is PlayerCombat playerCombat)
            {
                foreach (GameObject go in playerStatsOnly)
                {
                    go.SetActive(true);
                }

                PlayerPermanentStats playerStats = playerCombat.playerPermanentStats;

                playerFocusTMP.text = playerStats.focusBase.ToString();
                playerStats.UpdateThreshold();
                allyNameTMP.text = combatant.combatantName;
                allyLevelTMP.text = playerStats.level.ToString();
                allyAttackTMP.text = playerStats.attackBase.ToString();
                allyFendTMP.text = playerStats.fendBase.ToString();

                var previousXP = playerStats.XP;
                var targetXP = previousXP + XPEarned;
                allyXPTMP.text = previousXP.ToString();

                UpdateXPGainLayout();

                FieldEvents.LerpValues(previousXP, targetXP, 1, value =>
                {
                    playerStats.XP = Mathf.RoundToInt(value);
                    allyXPTMP.text = playerStats.XP.ToString();
                    allyXPRemainderTMP.text = (playerStats.XPThreshold - playerStats.XP).ToString();

                    UpdateXPGainLayout();

                    if (playerStats.XP >= playerStats.XPThreshold)
                    {
                        playerStats.LevelUp();
                        allyLevelTMP.text = playerStats.level.ToString();
                        allyAttackTMP.text = playerStats.attackBase.ToString();
                        allyFendTMP.text = playerStats.fendBase.ToString();
                        playerFocusTMP.text = playerStats.focusBase.ToString();
                    }
                });
            }

            else

            {
                PartyMemberCombat partyMemberCombat = combatant as PartyMemberCombat;

                foreach (GameObject go in playerStatsOnly)
                { go.SetActive(false); }

                PartyMemberSO partyMemberSO = partyMemberCombat.partyMemberSO;

                partyMemberSO.UpdateThreshold();
                allyNameTMP.text = combatant.combatantName;
                allyLevelTMP.text = partyMemberSO.level.ToString();
                allyAttackTMP.text = partyMemberSO.attackBase.ToString();
                allyFendTMP.text = partyMemberSO.fendBase.ToString();

                var previousXP = partyMemberSO.XP;
                var targetXP = previousXP + XPEarned;
                allyXPTMP.text = previousXP.ToString();

                UpdateXPGainLayout();

                FieldEvents.LerpValues(previousXP, targetXP, 1, value =>
                {
                    partyMemberSO.XP = Mathf.RoundToInt(value);
                    allyXPTMP.text = partyMemberSO.XP.ToString();
                    allyXPRemainderTMP.text = (partyMemberSO.XPThreshold - partyMemberSO.XP).ToString();

                    UpdateXPGainLayout();

                    if (partyMemberSO.XP >= partyMemberSO.XPThreshold)
                    {
                        partyMemberSO.LevelUp();
                        allyLevelTMP.text = partyMemberSO.level.ToString();
                        allyAttackTMP.text = partyMemberSO.attackBase.ToString();
                        allyFendTMP.text = partyMemberSO.fendBase.ToString();
                    }
                });
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
