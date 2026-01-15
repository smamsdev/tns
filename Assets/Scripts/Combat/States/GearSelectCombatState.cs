using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectCombatState : State
{
    public EquipSlotSelectMenu equipSlotSelectMenu;
    public GearSelectCombatMenu gearSelectCombatMenu;
    [SerializeField] Move equipGearMove; //player needs a move assigned to complete their turn

    public override IEnumerator StartState()
    {
        gearSelectCombatMenu.InstantiateUIGearSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, true);
        combatManager.combatMenuManager.gearSelectMenuDefaultButton.Select();
       // gearSelectCombatMenu.DefaultButtonSelected();
        yield return new WaitForEndOfFrame();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, false);
            combatManager.SetState(combatManager.equipSlotSelectState);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.equipSlotSelectState.lastButtonSelected, Color.white);
            Debug.Log("fix");
            //gearSelectCombatMenu.UICombatGearSlotHighlighed.Deselect();
            combatManager.equipSlotSelectState.lastButtonSelected.Select();
            CombatEvents.UpdateNarrator("");
        }
    }

    public void EquipSelectedGear(UICombatGearSlot uICombatGearSlot)
    {
        gearSelectCombatMenu.isGearSlotsInitialized = false;

        Debug.Log("rewordk");
       // var geartoEquip = uICombatGearSlot.gearSO;
       // var playerInventory = combatManager.playerCombat.playerInventory;
       //
       // if (geartoEquip.isCurrentlyEquipped)
       // {
       //     int index = playerInventory.inventorySO.equippedGear.IndexOf(geartoEquip);
       //
       //     if (index == equipSlotSelectMenu.equipSlotSelected.equipSlotNumber) //if a gear selected is already equipped in the currently selected slot, do nothing
       //     {
       //         return;
       //     }
       //
       //     else 
       //     {
       //         playerInventory.UnequipGearFromSlot(geartoEquip); //if the gear to equip is already in another slot, remove it first
       //         playerInventory.DestroyGearInstance(geartoEquip);
       //     }
       // }
       //
       // if (equipSlotSelectMenu.equipSlotSelected.gearEquipped != null) //if a gear already exists in that slot remove it before equipping a new one
       // {
       //     playerInventory.UnequipGearFromSlot(equipSlotSelectMenu.equipSlotSelected.gearEquipped);
       //     playerInventory.DestroyGearInstance(equipSlotSelectMenu.equipSlotSelected.gearEquipped);
       // }
       //
       // playerInventory.EquipGearToSlot(geartoEquip, equipSlotSelectMenu.equipSlotSelected.equipSlotNumber);
       // playerInventory.InstantiateNewEquippedGear(combatManager, geartoEquip);
       // combatManager.combatMenuManager.SetButtonNormalColor(equipSlotSelectMenu.equipSlotSelected.GetComponent<Button>(), Color.white);
       // gearSelectCombatMenu.ClearSlots();
       // gearSelectCombatMenu.isGearSlotsInitialized = false;
       // ApplyGearEquipMove();   
    }

    void ApplyGearEquipMove()
    {
        //combatManager.playerCombat.moveSelected = equipGearMove;
        Debug.Log("asdasd");
        //equipSlotSelectMenu.equipSlotSelected = equipSlotSelectMenu.uIGearEquipSlots[0];
        combatManager.SetState(combatManager.applyMove);
    }
}
