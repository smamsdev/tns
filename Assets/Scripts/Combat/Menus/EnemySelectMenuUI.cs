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
    public EnemySelectState enemySelectState;

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

            targetSelectButtonUI.button.onClick.AddListener(() => enemySelectState.CombatantSelected(targetSelectButtonUI));
            targetSelectButtonUI.onHighlighted = () =>
            {
                highlightedButtonIndex = targetSelectButtonUIs.IndexOf(targetSelectButtonUI);
                enemySelectState.TargetHighlighted(targetSelectButtonUI);
            };

            targetSelectButtonUI.onUnHighlighted = () => enemySelectState.TargetUnHighlighted(targetSelectButtonUI.combatant);

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
}
