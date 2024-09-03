using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GearSelect : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GearSelectUI gearSelectUI;

    bool inventoryMenuEnabled;

    public override IEnumerator StartState()
    {
        combatManager.CombatUIManager.ChangeMenuState(combatManager.CombatUIManager.GearSelectMenu);
        gearSelectUI.gearSlot1Button.Select();

        yield break;
    }

    public void InventoryMenuEnabled(bool toggle)  //used by button

    { 
        inventoryMenuEnabled = toggle;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))

        {
            if (inventoryMenuEnabled) 
            {
                StartCoroutine(FieldEvents.CoolDown(0.2f));
                combatManager.SetState(combatManager.gearSelect);
                inventoryMenuEnabled = false;
            }

            if (!inventoryMenuEnabled && !FieldEvents.isCooldown())
            {
                combatManager.SetState(combatManager.firstMove);
                CombatEvents.UpdateNarrator("");
                gearSelectUI.EnableFirstMoveButtons();
            }
        }

    }

}
