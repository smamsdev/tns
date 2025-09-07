using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipSlotSelectState : State
{
    [SerializeField] EquipSlotSelectMenu combatGearMenu;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.EquipSlotSelectMenu, true);
        combatGearMenu.DisplayEquipSlots();
        combatManager.combatMenuManager.gearSelectMenuFirstButton = buttonSelected;
        combatManager.combatMenuManager.gearSelectMenuFirstButton.Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.firstMove.buttonSelected, Color.white);
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.GearSelectMenu, false);
            combatManager.SetState(combatManager.tacticalSelectState);
            CombatEvents.UpdateNarrator("");
        }
    }
}
