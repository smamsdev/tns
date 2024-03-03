using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : MonoBehaviour
{
    [SerializeField] CombatManagerV3 combatManagerV3;

    [SerializeField] GameObject combatMenuContainer;
    [SerializeField] GameObject playerStatsContainer;

    public IEnumerator StartState()

    {
        yield return new WaitForSeconds(0.01f);
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.LockPlayerMovement.Invoke();

        CombatEvents.UpdateFighterPosition.Invoke(combatManagerV3.player, combatManagerV3.battleScheme.playerFightingPosition.transform.position, 1f);

        //enemy
        combatManagerV3.enemyGameObjectDefaultPosition = combatManagerV3.battleScheme.enemyGameObject.transform.position;
        //  combatManagerV3.battleScheme.enemyGameObject.transform.GetChild(0).GetComponent<Enemy>().enabled = true; //turn on Enemy script, you don't want this on for every 'enemy' in the scnene
        combatMenuContainer.SetActive(true); //combat menu container
        playerStatsContainer.SetActive(true); //player stats container

        combatManagerV3.UpdateFighterPosition(combatManagerV3.battleScheme.enemyGameObject, combatManagerV3.battleScheme.enemyFightingPosition.transform.position, 1);
        combatManagerV3.battleScheme.enemyGameObject.GetComponent<SortingGroup>().enabled = false;

        combatManagerV3.enemyRawAttackPower = 0;

        CombatEvents.InitializeEnemyPartsHP?.Invoke();

        yield return new WaitForSeconds(1);
        CombatEvents.InitializeEnemyHP?.Invoke();

        //player stats

        CombatEvents.InitializePlayerHP?.Invoke(combatManagerV3.playerStats.playerMaxHP);
       
        // combatManagerV3.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);    ????/


        combatManagerV3.playerStats.InitalisePlayerStats();


        combatManagerV3.SetBattleStateFirstMove();

        yield break;
    }

}

