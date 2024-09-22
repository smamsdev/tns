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
        //setup
        yield return new WaitForSeconds(0.01f);
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.isBattleMode = true;
        CombatEvents.LockPlayerMovement.Invoke();

        combatManager.CombatUIManager.playerFendScript.InitialiseFendUIPosition();
        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);

        //position player
        FieldEvents.isCameraFollow = false;
        yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);

        //enemy
        foreach (Enemy enemy in combatManager.enemy)
        {
            var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();

            GameObject newEnemyCombatUI = Instantiate(EnemyUIPrefab, combatManager.gameObject.transform);
            newEnemyCombatUI.name = "EnemyUI For " + enemy.name;

            enemy.enemyUI = newEnemyCombatUI.GetComponent<EnemyUI>();
            enemy.enemyUI.partsTargetDisplay.enemy = enemy;

            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);

            enemy.enemyUI.AnchorEnemyUIToEnemyGameObject(enemy.enemyFightingPosition);

            yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);
            enemyMovementScript.lookDirection = enemy.forceLookDirection;

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

            if (enemy.forceLookDirection == Vector2.right)
            {
                var pos = enemy.enemyUI.enemyAttackDisplay.transform.localPosition;
                pos.x = Mathf.Abs(pos.x);
                enemy.enemyUI.enemyAttackDisplay.transform.localPosition = pos;
            }

            yield return new WaitForSeconds(1);
        }

        //set ui elements
        combatUIContainer.SetActive(true);
        combatManager.player.GetComponent<PlayerCombatAnimations>().SetCombatMode();
        combatManager.playerCombatStats.InitialiseStats();
        playerStatsUIContainer.SetActive(true);
        combatManager.CombatUIManager.selectEnemyMenuScript.InitializeButtonSlots();
        FieldEvents.isCameraFollow = true;


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }
}

