using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;

    [SerializeField] GameObject combatMenuContainer;
    [SerializeField] GameObject playerStatsContainer;

    public override IEnumerator StartState()

    {
        yield return new WaitForSeconds(0.01f);
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.LockPlayerMovement.Invoke();

        CombatEvents.UpdateFighterPosition.Invoke(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);

        //enemy
        combatManager.enemyGameObjectDefaultPosition = combatManager.battleScheme.enemyGameObject.transform.position;
        //  combatManager.battleScheme.enemyGameObject.transform.GetChild(0).GetComponent<Enemy>().enabled = true; //turn on Enemy script, you don't want this on for every 'enemy' in the scnene
        combatMenuContainer.SetActive(true); //combat menu container
        playerStatsContainer.SetActive(true); //player stats container

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.battleScheme.enemyFightingPosition.transform.position, 1);
        combatManager.battleScheme.enemyGameObject.GetComponent<SortingGroup>().enabled = false;

        combatManager.enemyRawAttackPower = 0;

        CombatEvents.InitializeEnemyPartsHP?.Invoke();

        yield return new WaitForSeconds(1);
        CombatEvents.InitializeEnemyHP?.Invoke();

        //player stats

        CombatEvents.InitializePlayerHP?.Invoke(combatManager.playerStats.playerMaxHP);
       
        // combatManager.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);    ????/


        combatManager.playerStats.InitalisePlayerStats();


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }


}

