using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UICombatGearSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GearSO gear;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    public Button button;
    public CombatMenuManager combatMenuManager;

    public void OnSelect(BaseEventData eventData)
    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
        combatMenuManager.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.7f : 1f, Color.yellow);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        combatMenuManager.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.5f : 1f, Color.white);
    }

    public void DefaultButtonSelected() //put this here because OnSelect doesn't work at first
    {
        CombatEvents.UpdateNarrator(gear.gearDescription);
        combatMenuManager.SetTextAlpha(itemQuantity, gear.isCurrentlyEquipped ? 0.7f : 1f, Color.yellow);
    }
}
