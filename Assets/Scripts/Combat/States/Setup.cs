using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;

    [SerializeField] GameObject combatMenuContainer;
    [SerializeField] GameObject playerStatsUIContainer;

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

        combatManager.combatUIScript.playerFendScript.ShowFendDisplay(false);
        combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);
        combatMenuContainer.SetActive(true);
        playerStatsUIContainer.SetActive(true);

        combatManager.combatUIScript.selectEnemyMenuScript.InitializeButtonSlots();

        //enemy

        foreach (Enemy enemy in combatManager.enemy)

        {
            GameObject newEnemyCombatUI = Instantiate(EnemyUIPrefab, combatManager.gameObject.transform);
            newEnemyCombatUI.name = "EnemyUI For " + enemy.name;

            enemy.enemyUI = newEnemyCombatUI.GetComponent<EnemyUI>();
            enemy.enemyUI.partsTargetDisplay.enemy = enemy;
            enemy.enemyUI.combatManager = combatManager;


            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);

            enemy.SelectEnemyMove();
            enemy.enemyUI.AnchorEnemyUIToEnemyGameObject(enemy.enemyFightingPosition);
            combatManager.UpdateFighterPosition(enemy.gameObject, enemy.enemyFightingPosition.transform.position, 1f);

            yield return new WaitForSeconds(1);

            enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
            enemy.enemyUI.enemyStatsDisplay.InitializeEnemyStatsUI(enemy);
            enemy.enemyUI.partsTargetDisplay.InitializeEnemyPartsHP();

            if (combatManager.enemy[combatManager.selectedEnemy].attackTotal > 0)
            {
                enemy.enemyUI.enemyAttackDisplay.UpdateEnemyAttackDisplay(combatManager.enemy[combatManager.selectedEnemy].EnemyAttackTotal());
            }

        }
        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}

