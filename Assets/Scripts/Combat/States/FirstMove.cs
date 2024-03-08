using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject enemyAttackDisplay;
    [SerializeField] GameObject firstMoveContainer;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);
        firstMoveContainer.SetActive(true);

        combatManager.combatUIScript.ShowFirstMoveMenu();
        combatManager.playerMoveManager.firstMoveIs = 0;

        CombatEvents.ShowHideFendDisplay?.Invoke(false);
        CombatEvents.GetEnemyAttackPower?.Invoke();
        enemyAttackDisplay.SetActive(true);

        yield break;
    }

}
