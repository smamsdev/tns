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
        combatManager.playerAnimator = combatManager.player.GetComponent<Animator>();
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.isBattleMode = true;
        CombatEvents.LockPlayerMovement.Invoke();

        combatManager.CombatUIManager.playerFendScript.InitialiseFendUIPosition();
        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);

        //position player

        FieldEvents.isCameraFollow = false;
        yield return combatManager.combatMovement.MoveCombatant(combatManager.player.gameObject, combatManager.battleScheme.playerFightingPosition.transform.position);
        combatManager.playerAnimator.SetBool("isCombat", true);

        //enemy
        foreach (Enemy enemy in combatManager.enemy)
        {
            var enemyMovementScript = enemy.GetComponent<ActorMovementScript>();

            GameObject newEnemyCombatUI = Instantiate(EnemyUIPrefab, enemy.gameObject.transform);
            newEnemyCombatUI.transform.localPosition = Vector3.zero;

            newEnemyCombatUI.name = "EnemyUI For " + enemy.name;

            enemy.enemyUI = newEnemyCombatUI.GetComponent<EnemyUI>();
            enemy.enemyUI.partsTargetDisplay.enemy = enemy;
            enemy.enemyUI.partsTargetDisplay.combatManager = combatManager;
            enemy.enemyUI.enemyFendScript.combatManager = combatManager;

            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);

            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();

            yield return combatManager.combatMovement.MoveCombatant(enemy.gameObject, enemy.enemyFightingPosition.transform.position);
            enemyMovementScript.lookDirection = enemy.forceLookDirection;
            enemyMovementScript.actorRigidBody2d.bodyType = RigidbodyType2D.Kinematic;

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

            //flip UI elements based on look direction
            if (enemy.forceLookDirection == Vector2.right)
            {
                var flippedPos = enemy.enemyUI.enemyAttackDisplay.transform.localPosition;
                flippedPos.x = -flippedPos.x;
                enemy.enemyUI.enemyAttackDisplay.transform.localPosition = flippedPos;

                enemy.enemyUI.partsTargetDisplay.FlipTargetDisplay();
            }

            yield return new WaitForSeconds(1);
        }

        //set ui elements
        combatUIContainer.SetActive(true);
        combatManager.playerCombatStats.InitialiseStats();
        playerStatsUIContainer.SetActive(true);
        combatManager.CombatUIManager.selectEnemyMenuScript.InitializeButtonSlots();
        FieldEvents.isCameraFollow = true;


        combatManager.SetState(combatManager.firstMove);

        yield break;
    }
}

