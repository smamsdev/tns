using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearState : State
{
    public CombatGearSelectMenuUI combatGearSelectMenuUI;
    [SerializeField]MoveSO equipGearMoveSO; //player needs a move assigned to complete their turn

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
    }

    void GearEffectOnTurnEquipMove()
    {
        combatManager.playerCombat.moveSOSelected = equipGearMoveSO;
        combatManager.SetState(combatManager.applyMove);
    }


    public void OnInventorySlotSelected(InventorySlotUI inventorySlot)
    {
        int equipSlotIndex = combatManager.combatMenuManager.combatEquipSelectMenuUI.highlightedButtonIndex;
        combatManager.playerCombat.playerInventorySO.EquipGearToSlot(inventorySlot.gearInstance, equipSlotIndex);
        GearEffectOnTurnEquipMove();
    }
}
