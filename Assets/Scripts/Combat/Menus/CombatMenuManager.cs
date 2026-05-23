using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenuManager : MonoBehaviour
{
    public CombatManager combatManager;
    [SerializeField] TextMeshProUGUI narratorTMP;
    public StyleSelectMenuUI styleSelectMenuUI;
    public ActionSelectMenuUI actionSelectMenuUI;
    public TacticalSelectMenuUI tacticalSelectMenuUI;
    public EnemySelectMenuUI enemySelectMenuUI;
    public CombatEquipSelectMenuUI combatEquipSelectMenuUI;
    public CombatGearSelectMenuUI combatGearSelectMenuUI;
    public VictoryRewardsUI victoryRewardsUI;
    public GameObject menuParent;

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

        if (!menuParent.activeSelf || !this.gameObject.activeSelf)
        {
            menuParent.SetActive(true);
            this.gameObject.SetActive(true);
        }
    }

    public void ZeroAllMenuIndexes()
    {
        actionSelectMenuUI.highlightedButtonIndex = 0;
        styleSelectMenuUI.highlightedButtonIndex = 0;
        tacticalSelectMenuUI.highlightedButtonIndex = 0;
        enemySelectMenuUI.highlightedButtonIndex = 0;
        combatEquipSelectMenuUI.highlightedButtonIndex = 0;
        combatGearSelectMenuUI.highlightedButtonIndex = 0;
    }

    public void UpdateNarrator(string narratorText)
    {
        if (!narratorTMP.gameObject.activeSelf)
            narratorTMP.gameObject.SetActive(true);

        narratorTMP.text = narratorText;
    }

    public void SetGearSlotUIColor(InventorySlotUI inventorySlot, Color normalColor, float alpha)
    {
        FieldEvents.SetTextColor(inventorySlot.itemNameTMP, normalColor,alpha);
        FieldEvents.SetTextColor(inventorySlot.itemQuantityTMP, normalColor, alpha);
    }
}