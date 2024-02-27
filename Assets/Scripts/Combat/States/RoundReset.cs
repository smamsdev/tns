using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{

    public RoundReset(CombatManagerV3 _combatManagerV3) : base(_combatManagerV3)

    {
    }

    public override IEnumerator Start()
    {
        CombatEvents.ShowHideFendDisplay?.Invoke(false);

        combatManagerV3.combatUIScript.HideSecondMenu();
        combatManagerV3.combatUIScript.HideTargetMenu();

        combatManagerV3.playerStats.ResetAllMoveMods();
        combatManagerV3.player.GetComponent<GearEquip>().equippedGear[0].ResetAttackGear();
        combatManagerV3.player.GetComponent<GearEquip>().equippedGear[0].ResetFendGear();

        combatManagerV3.attackTargetMenuScript.attackTargetMenu.SetActive(false);
        combatManagerV3.attackTargetMenuScript.targetSelected = false;
        combatManagerV3.attackTargetMenuScript.targetIsSet = 0;

        CombatEvents.UpdateFendDisplay.Invoke(0);

        combatManagerV3.playerMoveManager.firstMoveIs = 0;
        combatManagerV3.playerMoveManager.secondMoveIs = 0;

        combatManagerV3.roundCount++;

        combatManagerV3.SetBattleStateFirstMove();

        yield break;
    }

    public override void Update()
    {

    }

}
