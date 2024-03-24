using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;

    [SerializeField] GameObject combatMenuContainer;
    [SerializeField] GameObject playerStatsUIContainer;
    [SerializeField] GameObject enemyAttackDisplay;

    [SerializeField] GameObject EnemyUIPrefab;

    public override IEnumerator StartState()

    {

       // for (int i = 0; i < combatManager.enemy.Length; i++)
       // {
       //     Instantiate(EnemyUIPrefab, combatManager.gameObject.transform);
       // }

        //setup
        yield return new WaitForSeconds(0.01f);
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.LockPlayerMovement.Invoke();

        //player
        combatManager.playerCombatStats.InitialiseStats();
        CombatEvents.InitializeEnemyPartsHP?.Invoke();
        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(false);
        combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);
        combatMenuContainer.SetActive(true);
        playerStatsUIContainer.SetActive(true);

        combatManager.combatUIScript.selectEnemyMenuScript.InitializeButtonSlots();

        //enemy

        // for (int i = 0; i < combatManager.enemy.Length; i++)
        // {
        //     Instantiate(EnemyUIPrefab, combatManager.gameObject.transform);
        // }

        foreach (Enemy enemy in combatManager.enemy)

        {
            enemy.enemyUI.enemyFendScript.ShowFendDisplay(false);
            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);

            enemy.SelectEnemyMove();
            enemy.enemyUI.AnchorEnemyUIToEnemyGameObject(enemy.enemyFightingPosition);
            combatManager.UpdateFighterPosition(enemy.gameObject, enemy.enemyFightingPosition.transform.position, 1f);

            yield return new WaitForSeconds(1);

            enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
            enemy.enemyUI.enemyStatsDisplay.InitializeEnemyHP(enemy);
 
            if (combatManager.enemy[combatManager.selectedEnemy].attackTotal > 0)
            {
                CombatEvents.UpdateEnemyAttackDisplay?.Invoke(combatManager.enemy[combatManager.selectedEnemy].EnemyAttackTotal());
            }

        }
        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}

