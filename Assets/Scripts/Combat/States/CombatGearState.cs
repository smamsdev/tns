using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGearState : State
{
    public CombatGearSelectMenuUI combatGearSelectMenuUI;
    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn

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

    void ApplyGearEquipMove()
    {
        //combatManager.playerCombat.moveSelected = equipGearMove;
        Debug.Log("asdasd");
        //combatGearSelectMenuUI.equipSlotSelected = combatGearSelectMenuUI.uIGearEquipSlots[0];
        combatManager.SetState(combatManager.applyMove);
    }
}
