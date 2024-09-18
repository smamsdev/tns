using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;

    [SerializeField] GameObject combatUIContainer;
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
        CombatEvents.isBattleMode = true;
        CombatEvents.LockPlayerMovement.Invoke();

        combatManager.CombatUIManager.playerFendScript.InitialiseFendUIPosition();
        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);
        combatManager.UpdateFighterPosition(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position, 1f);
        combatUIContainer.SetActive(true);
        combatManager.playerCombatStats.InitialiseStats();
        playerStatsUIContainer.SetActive(true);

        combatManager.CombatUIManager.selectEnemyMenuScript.InitializeButtonSlots();

        //enemy

        foreach (Enemy enemy in combatManager.enemy)

        {
            GameObject newEnemyCombatUI = Instantiate(EnemyUIPrefab, combatManager.gameObject.transform);
            newEnemyCombatUI.name = "EnemyUI For " + enemy.name;

            enemy.enemyUI = newEnemyCombatUI.GetComponent<EnemyUI>();
            enemy.enemyUI.partsTargetDisplay.enemy = enemy;

            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);

            enemy.enemyUI.AnchorEnemyUIToEnemyGameObject(enemy.enemyFightingPosition);
            combatManager.UpdateFighterPosition(enemy.gameObject, enemy.enemyFightingPosition.transform.position, 1f);

            yield return new WaitForSeconds(1);

            enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
            enemy.enemyUI.enemyStatsDisplay.InitializeEnemyStatsUI(enemy);
            enemy.enemyUI.partsTargetDisplay.InitializeEnemyPartsHP();

            enemy.SelectEnemyMove();

            if (enemy.attackTotal > 0)
            {
                enemy.enemyUI.enemyAttackDisplay.UpdateEnemyAttackDisplay(enemy.EnemyAttackTotal());
                enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(true);
            }

            if (enemy.fendTotal > 0)
            {
                enemy.enemyUI.enemyFendScript.UpdateFendDisplay(enemy.fendTotal);
            }

        }
        combatManager.SetState(combatManager.firstMove);

        yield break;
    }

}

