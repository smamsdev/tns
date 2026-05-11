using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipSlotSelectState : State
{
    [SerializeField] CombatEquipSelectMenuUI combatEquipSelectMenuUI;

    public override IEnumerator StartState()
    {
        combatEquipSelectMenuUI.InitialiseEquipSlots();
        combatEquipSelectMenuUI.menuButtons[combatEquipSelectMenuUI.highlightedButtonIndex].Select();
        combatEquipSelectMenuUI.DisplayMenu(true);
        yield break;
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FieldEvents.CoolDown(0.2f));
            combatEquipSelectMenuUI.DisplayMenu(false);
            combatManager.SetState(combatManager.tacticalSelectState);
            CombatEvents.UpdateNarrator("");
        }
    }
}