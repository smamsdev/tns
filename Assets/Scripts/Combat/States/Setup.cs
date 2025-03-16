using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject combatUIContainer;
    [SerializeField] GameObject playerStatsUIContainer;
    [SerializeField] GameObject EnemyUIPrefab;
    [SerializeField] GameObject allyUI;
    [SerializeField] GameObject playerFendContainerPrefab;

    public override IEnumerator StartState()

    {
        yield return HouseKeeping();
        yield return combatManager.PositionCombatant(combatManager.player, combatManager.battleScheme.playerFightingPosition.transform.position);
        combatManager.playerAnimator.SetBool("isCombat", true);
        combatManager.player.GetComponent<PlayerMovementScript>().lookDirection = combatManager.battleScheme.playerDefaultLookDirection;

        foreach (Enemy enemy in combatManager.enemies)
        {
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.targetToAttack = combatManager.allAllies[Random.Range(0, combatManager.allAllies.Count)];
            SetDefaultLookDirectionAndType(enemy);
            SetEnemyUI(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            ally.targetToAttack = combatManager.enemies[Random.Range(0, combatManager.enemies.Count)];
            SetDefaultLookDirectionAndType(ally);
            SetAllyUI(ally);
        }

        FieldEvents.isCameraFollow = true;

        foreach (Ally ally in combatManager.allies)
        {
            SelectAndDisplayAllyMove(ally);
            ally.targetToAttack = combatManager.enemies[Random.Range(0, combatManager.enemies.Count)];
        }

        foreach (Enemy enemy in combatManager.enemies)
        {
            SelectAndDisplayEnemyMove(enemy);
        }

        yield return new WaitForSeconds(1);
        yield return SetPlayerUI();
        combatManager.SetState(combatManager.firstMove);
    }

    void SetEnemyUI(Enemy enemy)
    {
            GameObject newEnemyCombatUI = Instantiate(EnemyUIPrefab, enemy.gameObject.transform);
            newEnemyCombatUI.transform.localPosition = Vector3.zero;
            newEnemyCombatUI.name = "EnemyUI For " + enemy.combatantName;
            enemy.enemyUI = newEnemyCombatUI.GetComponent<EnemyUI>();
            enemy.enemyUI.partsTargetDisplay.enemy = enemy;
            enemy.enemyUI.partsTargetDisplay.combatManager = combatManager;
            enemy.enemyUI.fendScript.combatManager = combatManager;
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(false);
            enemy.enemyUI.enemyDamageTakenDisplay.DisableEnemyDamageDisplay();
            enemy.enemyUI.enemyStatsDisplay.InitializeEnemyStatsUI(enemy);
            enemy.enemyUI.partsTargetDisplay.InitializeEnemyPartsHP();
            enemy.enemyUI.enemyStatsDisplay.ShowEnemyStatsDisplay(true);
    }

    void SelectAndDisplayEnemyMove(Enemy enemy)
    {
        enemy.SelectMove();
        enemy.moveSelected.LoadMoveStats(enemy, combatManager);

        if (enemy.attackTotal > 0)
        {
            enemy.enemyUI.enemyAttackDisplay.UpdateEnemyAttackDisplay(enemy.attackTotal);
            enemy.enemyUI.enemyAttackDisplay.ShowAttackDisplay(true);
        }

        if (enemy.fendTotal > 0)
        {
            enemy.enemyUI.fendScript.ShowFendDisplay(true);
            enemy.enemyUI.fendScript.UpdateFendText(enemy.fendTotal);
        }

        //flip UI elements based on look direction
        if (enemy.forceLookDirection == Vector2.right)
        {
            var flippedPos = enemy.enemyUI.enemyAttackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            enemy.enemyUI.enemyAttackDisplay.transform.localPosition = flippedPos;
            enemy.enemyUI.partsTargetDisplay.FlipTargetDisplay();
        }
    }

    void SetDefaultLookDirectionAndType(Combatant _combatant)
    {
        GameObject combatantGO = _combatant.gameObject;
        var combatantMovementScript = combatantGO.GetComponent<ActorMovementScript>();
        var combatant = combatantGO.GetComponent<Combatant>();
        combatantMovementScript.lookDirection = combatant.forceLookDirection;
        combatantMovementScript.actorRigidBody2d.bodyType = RigidbodyType2D.Kinematic;
        _combatant.GetComponent<Animator>().SetBool("isCombat", true);
    }

    void SetAllyUI(Ally ally)
    {
            allyUI.name = "AllyUI For " + ally.combatantName;
            ally.allyUI = allyUI.GetComponent<AllyUI>();
            ally.allyUI.fendScript.combatManager = combatManager;
            ally.allyUI.fendScript.UpdateFendText(ally.fendTotal);
            ally.allyUI.allyStatsDisplay.InitializeAllyStatsUI(ally);
            ally.allyUI.allyStatsDisplay.ShowAllyStatsDisplay(false);
            ally.allyUI.allyDamageTakenDisplay.DisableAllyDamageDisplay();

            ally.allyUI.allyStatsDisplay.ShowAllyStatsDisplay(true);
    }

    void SelectAndDisplayAllyMove(Ally ally)

    {
        ally.SelectMove();
        ally.moveSelected.LoadMoveStats(ally, combatManager);

        if (ally.attackTotal > 0)
        {
            ally.allyUI.allyAttackDisplay.UpdateAllyAttackDisplay(ally.AllyAttackTotal());
            ally.allyUI.allyAttackDisplay.ShowAttackDisplay(true);
        }

        if (ally.fendTotal > 0)
        {
            ally.allyUI.fendScript.UpdateFendText(ally.fendTotal);
        }

        //flip UI elements based on look direction
        if (ally.forceLookDirection == Vector2.right)
        {
            var flippedPos = ally.allyUI.allyAttackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            ally.allyUI.allyAttackDisplay.transform.localPosition = flippedPos;

        }
    }

    IEnumerator HouseKeeping()
    {
        yield return new WaitForSeconds(0.01f);
        combatManager.playerAnimator = combatManager.player.GetComponent<Animator>();
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.isBattleMode = true;
        CombatEvents.LockPlayerMovement.Invoke();
        FieldEvents.isCameraFollow = false;
        GameObject newFendContainer = Instantiate(playerFendContainerPrefab, combatManager.player.gameObject.transform);
        newFendContainer.name = "Player Fend Container";
        combatManager.CombatUIManager.playerFendScript = newFendContainer.GetComponent<FendScript>();
        combatManager.CombatUIManager.playerDamageTakenDisplay = newFendContainer.GetComponent<PlayerDamageTakenDisplay>();
        combatManager.CombatUIManager.playerFendScript.combatManager = combatManager;
        combatManager.CombatUIManager.playerFendScript.ShowFendDisplay(false);
    }

    IEnumerator SetPlayerUI()
    {        
        combatUIContainer.SetActive(true);
        combatManager.playerCombat.InitialiseStats();
        playerStatsUIContainer.SetActive(true);
        combatManager.CombatUIManager.selectEnemyMenuScript.InitializeButtonSlots();

        yield break;
    }
}