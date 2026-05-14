using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectMenuUI : CombatMenu
{
    public CombatManager combatManager;
    [SerializeField] GameObject enemySelectButtonPrefab;
    [SerializeField] GameObject targetSelectButtonsParent;
    public List<TargetSelectButtonUI> targetSelectButtonUIs = new List<TargetSelectButtonUI>();

    public void InitializeButtonSlots(List<Combatant> targetList)
    {
        DestroyAllUI();

        for (int i = 0; i < targetList.Count; i++)
        {
            GameObject enemySelectButtonGO = Instantiate(enemySelectButtonPrefab, targetSelectButtonsParent.transform);
            TargetSelectButtonUI targetSelectButtonUI = enemySelectButtonGO.GetComponent<TargetSelectButtonUI>();
            targetSelectButtonUI.combatant = combatManager.enemies[i];
            targetSelectButtonUI.tmp.text = targetSelectButtonUI.combatant.combatantName;
            enemySelectButtonGO.name = targetSelectButtonUI.combatant.combatantName;

            targetSelectButtonUI.button.onClick.AddListener(() => TargetSelected(targetSelectButtonUI));
            targetSelectButtonUI.onHighlighted = () => TargetHighlighted(targetSelectButtonUI);
            targetSelectButtonUI.onUnHighlighted = () => TargerUnHighlighted(targetSelectButtonUI);

            menuButtons.Add(targetSelectButtonUI.button);
            targetSelectButtonUIs.Add(targetSelectButtonUI);
        }

        FieldEvents.SetGridNavigationWrapAroundHorizontal(menuButtons, 3);
    }

    void DestroyAllUI()
    {
        menuButtons.Clear();
        targetSelectButtonUIs.Clear();

        for (int i = targetSelectButtonsParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(targetSelectButtonsParent.transform.GetChild(i).gameObject);
        }
    }

    void TargetHighlighted(TargetSelectButtonUI targetSelectButtonUI)
    {
        highlightedButtonIndex = targetSelectButtonUIs.IndexOf(targetSelectButtonUI);

        combatManager.cameraFollow.transformToFollow = targetSelectButtonUI.combatant.transform;
        var combatantUI = targetSelectButtonUI.combatant.combatantUI;
        combatantUI.statsDisplay.ShowStatsDisplay(true);

        combatantUI.selectedAnimator.SetBool("Flash", true);
        targetSelectButtonUI.combatant.combatantUI.DisplayCombatantMove(targetSelectButtonUI.combatant);

        Vector2 direction = (targetSelectButtonUI.combatant.transform.position - combatManager.playerCombat.transform.position).normalized;
        combatManager.playerCombat.CombatLookDirX = (int)Mathf.Sign(direction.x);
    }

    void TargerUnHighlighted(TargetSelectButtonUI targetSelectButtonUI)
    {
        var combatantUI = targetSelectButtonUIs[highlightedButtonIndex].combatant.combatantUI;

        combatantUI.selectedAnimator.SetBool("Flash", false);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
        combatantUI.attackDisplay.ShowAttackDisplay(targetSelectButtonUIs[highlightedButtonIndex].combatant, false);
        combatantUI.fendScript.ShowFendDisplay(targetSelectButtonUIs[highlightedButtonIndex].combatant, false);
    }

    public void TargetSelected(TargetSelectButtonUI targetSelectButtonUI)
    {
        Debug.Log("todo");
    }
}
