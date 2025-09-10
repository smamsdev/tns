using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHealingStrip : Gear
{
    public override IEnumerator ApplyGear()
    {
        var targetToHeal = combatManager.playerCombat;
        combatManager.cameraFollow.transformToFollow = targetToHeal.transform;
        CombatEvents.UpdateNarrator("Healing Strip");
        yield return new WaitForSeconds(.5f);

        StartCoroutine(targetToHeal.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(69, true));
        targetToHeal.UpdateHP(69);

        yield return new WaitForSeconds(1.5f);
        CombatEvents.UpdateNarrator("");
    }

    public override void OnEquipGear()
    {
        return;
    }
}
