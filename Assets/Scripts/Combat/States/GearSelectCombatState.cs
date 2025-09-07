using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearSelectCombatState : State
{
    [SerializeField] Button equipNoneOption;
    public EquipSlotSelectMenu equipSlotSelectMenu;
    public GearSelectCombatMenu gearSelectCombatMenu;

    public override IEnumerator StartState()
    {
        gearSelectCombatMenu.InstantiateUIGearSlots();
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, true);
        yield return new WaitForEndOfFrame();
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //StartCoroutine(FieldEvents.CoolDown(0.2f));
            //combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.buttonSelected, Color.white);
            //combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, false);
            //combatManager.SetState(combatManager.tacticalSelectState);
            //CombatEvents.UpdateNarrator("");
        }
    }

    public void equipNone()
    {
        if (equipSlotSelectMenu.equipSlotSelected.gearEquipped == null)
        {
            //equipSlotSelectState.ResetStateGearSelect();
            Debug.Log("Fix");
        }

        else
        {
            //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped);
            Debug.Log("fix this");
            combatManager.combatMenuManager.SetButtonNormalColor(equipSlotSelectMenu.equipSlotSelected.GetComponent<Button>(), Color.white);
            equipSlotSelectMenu.equipSlotSelected.gearEquipped = null;
            ApplyGearSelected();
        }
    }

    public void equipSelectedGear(InventorySlot inventorySlot)
    {
        if (!inventorySlot.gear.isCurrentlyEquipped) //if gear selected is already equipped do nothing
        {
            var geartoEquip = inventorySlot.gear;

            if (equipSlotSelectMenu.equipSlotSelected.gearEquipped != null)
            {
                Debug.Log("fix this");
                //playerInventory.UnequipGearFromSlot(gearEquipSlotSelected.gearEquipped); //if a gear exists remove it before equipping a new one
            }

            Debug.Log("fix this");
            //playerInventory.EquipGearToSlot(geartoEquip, gearEquipSlotSelected.equipSlotNumber);
            combatManager.combatMenuManager.SetButtonNormalColor(equipSlotSelectMenu.equipSlotSelected.GetComponent<Button>(), Color.white);

            ApplyGearSelected();
        }
    }

    void ApplyGearSelected()
    {
        Debug.Log("fix");
        //combatManager.playerCombat.moveSelected = equipGearMove;
        //inventoryMenu.SetActive(false);
        //equipSlotSelectMenu.equipSlotSelected = uIGearEquipSlots[0];
        //combatManager.SetState(combatManager.applyMove);
    }
}
