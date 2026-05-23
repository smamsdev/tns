using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipSlotSelectState : State
{
    [SerializeField] CombatEquipSelectMenuUI combatEquipSelectMenuUI;
    [SerializeField] MoveSO gearChangeSO;

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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GearInstance gearToUnequip = combatEquipSelectMenuUI.equipSlots[combatEquipSelectMenuUI.highlightedButtonIndex].gearInstance;

            if (!gearToUnequip.isCurrentlyEquipped)
                return;

            combatManager.playerCombat.playerInventorySO.UnequipGear(gearToUnequip, combatManager.playerCombat);
            combatManager.applyMoveState.dynamicMoveName = "Uneqquipping " + gearToUnequip.gearSO.GearName;
            combatEquipSelectMenuUI.highlightedButtonIndex = 0;
            combatManager.playerCombat.InstantiateMoveBehaviour(gearChangeSO);

            combatManager.SetState(combatManager.applyMoveState);
        }
    }

    public void OnEquipSlotSelected(InventorySlotUI gearEquipSlotSelected)
    {
        combatManager.SetState(combatManager.combatGearState);
        combatManager.combatMenuManager.SetGearSlotUIColor(gearEquipSlotSelected, Color.yellow, 1);
    }
}