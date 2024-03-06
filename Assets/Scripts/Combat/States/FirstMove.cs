using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManagerV3 combatManagerV3;
    [SerializeField] GameObject enemyAttackDisplay;
    [SerializeField] GameObject firstMoveContainer;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);
        firstMoveContainer.SetActive(true);

        combatManagerV3.combatUIScript.ShowFirstMoveMenu();
        combatManagerV3.playerMoveManager.firstMoveIs = 0;

        CombatEvents.ShowHideFendDisplay?.Invoke(false);
        CombatEvents.GetEnemyAttackPower?.Invoke();
        enemyAttackDisplay.SetActive(true);

        yield break;
    }

}
