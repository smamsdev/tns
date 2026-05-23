using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearState : State
{
    public CombatGearSelectMenuUI combatGearSelectMenuUI;
    [SerializeField]MoveSO gearChangeSO; //player needs a move assigned to complete their turn

    public override IEnumerator StartState()
    {
        combatGearSelectMenuUI.InitialiseInventoryUI();
        combatGearSelectMenuUI.DisplayMenu(true);
        combatGearSelectMenuUI.inventorySlotUIs[combatGearSelectMenuUI.highlightedButtonIndex].button.Select();
       // gearSelectCombatMenu.DefaultButtonSelected();
        yield return new WaitForEndOfFrame();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatGearSelectMenuUI.DisplayMenu(false); 
            combatManager.SetState(combatManager.equipSlotSelectState);
            combatManager.combatMenuManager.UpdateNarrator("");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GearInstance gearToUnequip = combatGearSelectMenuUI.inventorySlotUIs[combatGearSelectMenuUI.highlightedButtonIndex].gearInstance;

            if (!gearToUnequip.isCurrentlyEquipped)
                return;

            combatManager.playerCombat.playerInventorySO.UnequipGear(gearToUnequip, combatManager.playerCombat);
            combatManager.applyMoveState.dynamicMoveName = "Unequipping " + gearToUnequip.gearSO.GearName;
            combatGearSelectMenuUI.highlightedButtonIndex = 0;
            combatManager.playerCombat.InstantiateMoveBehaviour(gearChangeSO);

            combatManager.SetState(combatManager.applyMoveState);
        }
    }

    public void OnInventorySlotSelected(InventorySlotUI inventorySlot)
    {
        int equipSlotIndex = combatManager.combatMenuManager.combatEquipSelectMenuUI.highlightedButtonIndex;
        combatManager.playerCombat.playerInventorySO.EquipGearToSlot(inventorySlot.gearInstance, equipSlotIndex, combatManager.playerCombat);
        combatManager.applyMoveState.dynamicMoveName = "Equipping " + inventorySlot.gearInstance.gearSO.GearName;
        combatManager.playerCombat.InstantiateMoveBehaviour(gearChangeSO);
        combatManager.SetState(combatManager.applyMoveState);
    }
}
