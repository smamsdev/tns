using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GearSelectState : State
{
    [SerializeField] CombatGearMenu combatGearMenu;
    bool inventoryMenuEnabled;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, true);
        combatGearMenu.DisplayEquipSlots();

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
                ResetStateGearSelect();

            }

            if (!inventoryMenuEnabled && !FieldEvents.isCooldown())
            {
                RevertToFirstMove();
            }
        }
    }

    public void ResetStateGearSelect()
    {
        combatManager.SetState(combatManager.gearSelectState);
        inventoryMenuEnabled = false;
        var combatMenuManager = combatManager.combatMenuManager;
        combatMenuManager.SetButtonNormalColor(buttonSelected, Color.white);
    }

    void RevertToFirstMove()
    {
        combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.buttonSelected, Color.white);
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, false);
        combatManager.SetState(combatManager.firstMove);
        CombatEvents.UpdateNarrator("");
    }
}
