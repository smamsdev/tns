using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GearSelect : State
{
    [SerializeField] CombatManagerV3 combatManagerV3;
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
                combatManagerV3.SetState(combatManagerV3.gearSelect);
                combatInventory.HideInventoryMenu();
            }

            if (!inventoryMenuEnabled && !FieldEvents.isCooldown())
            {
                combatManagerV3.SetState(combatManagerV3.firstMove);
                equippedGearDisplayUI.EnableFirstMoveButtons();
            }
        }

    }

}
