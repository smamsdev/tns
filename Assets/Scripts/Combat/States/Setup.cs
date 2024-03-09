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
        combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);

        //enemy
        combatManager.enemyGameObjectDefaultPosition = combatManager.battleScheme.enemyGameObject.transform.position;
        combatMenuContainer.SetActive(true);
        playerStatsContainer.SetActive(true); 

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.battleScheme.enemyFightingPosition.transform.position, 1);

        combatManager.enemyRawAttackPower = 0;

        CombatEvents.InitializeEnemyPartsHP?.Invoke();
        combatManager.combatUIScript.fendScript.ShowHideFendDisplay(false);

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

