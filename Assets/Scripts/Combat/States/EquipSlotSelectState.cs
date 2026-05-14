using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipSlotSelectState : State
{
    [SerializeField] CombatEquipSelectMenuUI combatEquipSelectMenuUI;

    public override IEnumerator StartState()
    {
        combatEquipSelectMenuUI.DisplayMenu(true);
        combatEquipSelectMenuUI.InitialiseEquipSlots();

        combatEquipSelectMenuUI.menuButtons[combatEquipSelectMenuUI.highlightedButtonIndex].Select();
        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatEquipSelectMenuUI.DisplayMenu(false);
            combatEquipSelectMenuUI.highlightedButtonIndex = 0;
            combatManager.SetState(combatManager.tacticalSelectState);
            combatManager.combatMenuManager.UpdateNarrator("");
        }
    }

    public void OnEquipSlotSelected(InventorySlotUI gearEquipSlotSelected)
    {
        combatManager.SetState(combatManager.combatGearState);
        combatManager.combatMenuManager.SetGearSlotUIColor(gearEquipSlotSelected, Color.yellow, 1);
    }
}