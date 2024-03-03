using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMove : MonoBehaviour
{
    [SerializeField] CombatManagerV3 combatManagerV3;
    [SerializeField] GameObject enemyAttackDisplay;

    public  IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.1f);
        combatManagerV3.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        combatManagerV3.combatUIScript.ShowFirstMoveMenu();
        combatManagerV3.playerMoveManager.firstMoveIs = 0;

        CombatEvents.ShowHideFendDisplay?.Invoke(false);
        CombatEvents.GetEnemyAttackPower?.Invoke();
        enemyAttackDisplay.SetActive(true);

        yield break;
    }



}
