using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHealingStrip : GearMonoBehaviour
{
    public override IEnumerator ApplyGear()
    {
        var targetToHeal = combatManager.playerCombat;
        combatManager.cameraFollow.transformToFollow = targetToHeal.transform;

        Debug.Log("this shit shit vvv");
        combatManager.combatMenuManager.UpdateNarrator("Healing Strip");
        yield return new WaitForSeconds(.5f);

        //StartCoroutine(targetToHeal.combatantUI.damageTakenDisplay.ShowDamageDisplayCoro(69, true));
        targetToHeal.UpdateHP(69);

        yield return new WaitForSeconds(1.5f);
        combatManager.combatMenuManager.UpdateNarrator("");
    }

    public override void OnEquipGear()
    {
        return;
    }
}
