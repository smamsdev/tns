using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICombatGearSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GearSO gearSO;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    public Button button;
    public CombatMenuManager combatMenuManager;
    public GearSelectCombatMenu gearSelectCombatMenu;

    public void OnSelect(BaseEventData eventData)
    {
        CombatEvents.UpdateNarrator(gearSO.gearDescription);
        combatMenuManager.SetTextAlpha(itemQuantity, gearSO.isCurrentlyEquipped ? 0.7f : 1f, Color.yellow);
        combatMenuManager.gearSelectMenuDefaultButton = button;
        gearSelectCombatMenu.UICombatGearSlotHighlighed = this;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        combatMenuManager.SetTextAlpha(itemQuantity, gearSO.isCurrentlyEquipped ? 0.5f : 1f, Color.white);
    }

    public void Deselect()
    {
        combatMenuManager.SetTextAlpha(itemQuantity, gearSO.isCurrentlyEquipped ? 0.5f : 1f, Color.white);
    }
}
