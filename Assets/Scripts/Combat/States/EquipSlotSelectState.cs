using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipSlotSelectState : State
{
    [SerializeField] EquipSlotSelectMenu equipSlotSelectMenu;

    public override IEnumerator StartState()
    {
        combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.EquipSlotSelectMenu, true);
        equipSlotSelectMenu.DisplayEquipSlots();
        combatManager.combatMenuManager.equipSlotSelectMenuDefaultButton.Select();

        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatManager.combatMenuManager.DisplayMenuGO(combatManager.combatMenuManager.EquipSlotSelectMenu, false);
            combatManager.SetState(combatManager.tacticalSelectState);
            combatManager.combatMenuManager.SetButtonNormalColor(combatManager.tacticalSelectState.lastButtonSelected, Color.white);
            combatManager.tacticalSelectState.lastButtonSelected.Select();
            CombatEvents.UpdateNarrator("");
        }
    }
}