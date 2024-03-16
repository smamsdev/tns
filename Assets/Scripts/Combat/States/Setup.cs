using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;

    [SerializeField] GameObject combatMenuContainer;
    [SerializeField] GameObject playerStatsContainer;
    [SerializeField] GameObject enemyAttackDisplay;

    public override IEnumerator StartState()

    {
        combatManager.playerStats.InitalisePlayerStats();
        CombatEvents.InitializePlayerHP?.Invoke(combatManager.playerStats.playerCurrentHP);

        yield return new WaitForSeconds(0.01f);
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.LockPlayerMovement.Invoke();
        combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);

        //enemy
        combatManager.enemyGameObjectDefaultPosition = combatManager.battleScheme.enemyGameObject.transform.position;
        combatMenuContainer.SetActive(true);
        playerStatsContainer.SetActive(true);

        combatManager.UpdateFighterPosition(combatManager.battleScheme.enemyGameObject, combatManager.battleScheme.enemyFightingPosition.transform.position, 1f);

        combatManager.enemyRawAttackPower = 0;

        CombatEvents.InitializeEnemyPartsHP?.Invoke();
        combatManager.combatUIScript.playerFendScript.ShowHideFendDisplay(false);
        combatManager.combatUIScript.enemyFendScript.ShowHideFendDisplay(false);

        yield return new WaitForSeconds(1);
        CombatEvents.InitializeEnemyHP?.Invoke();

        CombatEvents.InitializePlayerHP?.Invoke(combatManager.playerStats.playerCurrentHP);

        combatManager.enemy.SelectEnemyMove();
        CombatEvents.UpdateEnemyFendDisplay?.Invoke(combatManager.enemy.fendTotal);

        if (combatManager.enemy.attackTotal > 0)
        {
            CombatEvents.UpdateEnemyAttackDisplay?.Invoke(combatManager.enemy.EnemyAttackTotal());
            enemyAttackDisplay.SetActive(true);
        }


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }


}

