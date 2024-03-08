using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GearSelect : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] EquippedGearDisplayUI equippedGearDisplayUI;
    [SerializeField] CombatInventoryMenu combatInventory;

    bool inventoryMenuEnabled;

    public override IEnumerator StartState()
    {
        equippedGearDisplayUI.ShowGearSelectionMenu();
                        inventoryMenuEnabled = false;

        yield break;
    }

    public void InventoryMenuEnabled()

    { 
     inventoryMenuEnabled = true;
    }

    public void InventoryMenuDisabled()

    {
        inventoryMenuEnabled = false;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            if (inventoryMenuEnabled) 
            {
                StartCoroutine(FieldEvents.CoolDown(0.2f));
                combatManager.SetState(combatManager.gearSelect);
                combatInventory.HideInventoryMenu();
            }

            if (!inventoryMenuEnabled && !FieldEvents.isCooldown())
            {
                combatManager.SetState(combatManager.firstMove);
                CombatEvents.UpdateNarrator("");
                equippedGearDisplayUI.EnableFirstMoveButtons();
            }
        }

    }

}
