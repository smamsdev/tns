using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public CombatManager combatManager;
    public TextMeshProUGUI narratorTMP;

    public ActionSelectMenuUI actionSelectMenuUI;
    public StyleSelectMenuUI styleSelectMenuUI;
    public TacticalSelectMenuUI tacticalSelectMenuUI;
    public EnemySelectMenuUI enemySelectMenuUI;
    public CombatEquipSelectMenuUI combatEquipSelectMenuUI;
    public CombatGearSelectMenuUI combatGearSelectMenuUI;
    public VictoryRewardsUI victoryRewardsUI;

    public void DisableAllMenus()
    {
        actionSelectMenuUI.DisplayMenu(false);
        styleSelectMenuUI.DisplayMenu(false);
        tacticalSelectMenuUI.DisplayMenu(false);
        enemySelectMenuUI.DisplayMenu(false);
        combatEquipSelectMenuUI.DisplayMenu(false);
        combatGearSelectMenuUI.DisplayMenu(false);
        victoryRewardsUI.DisplayMenu(false);
        narratorTMP.gameObject.SetActive(false);
    }

    public void UpdateNarrator(string narratorText)
    {
        if (!narratorTMP.gameObject.activeSelf)
            narratorTMP.gameObject.SetActive(true);

        narratorTMP.text = narratorText;
    }
}