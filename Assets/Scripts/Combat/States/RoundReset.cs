using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundReset : State
{
    [SerializeField] CombatManager combatManager;

    public override IEnumerator StartState()
    {
        combatManager.combatUIScript.playerFendScript.ShowHideFendDisplay(false);

        combatManager.combatUIScript.HideSecondMenu();
        combatManager.combatUIScript.HideTargetMenu();

        combatManager.playerStats.ResetAllMoveMods();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetAttackGear();
        //combatManager.player.GetComponent<EquippedGear>().equippedGear[0].ResetFendGear();

        combatManager.attackTargetMenuScript.attackTargetMenu.SetActive(false);
        combatManager.attackTargetMenuScript.targetSelected = false;
        combatManager.attackTargetMenuScript.targetIsSet = 0;

        combatManager.combatUIScript.playerFendScript.UpdateFendText(0);
        combatManager.combatUIScript.playerFendScript.FendIconAnimationState(0);
        combatManager.combatUIScript.enemyFendScript.FendIconAnimationState(0);
        combatManager.combatUIScript.enemyFendScript.ShowHideFendDisplay(false);



        combatManager.playerMoveManager.firstMoveIs = 0;
        combatManager.playerMoveManager.secondMoveIs = 0;

        combatManager.roundCount++;

        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}
