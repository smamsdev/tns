using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GearSelectState : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GearSelectMenu gearSelectMenu;
    [SerializeField] GearSelectUI gearSelectUI;

    bool inventoryMenuEnabled;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.ChangeMenuState(combatManager.combatMenuManager.GearSelectMenu);
        gearSelectMenu.DisplayEquipSlots();

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
                RevertState();
            }
        }
    }

    public void ResetStateGearSelect()
    {
        combatManager.SetState(combatManager.gearSelectState);
        inventoryMenuEnabled = false;
    }

    void RevertState()
    {
        combatManager.SetState(combatManager.firstMove);
        CombatEvents.UpdateNarrator("");
    }
}
