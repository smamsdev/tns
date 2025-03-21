using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject combatantUIPrefab;
    [SerializeField] CombatMenuManager combatMenuManager;
    PlayerCombat playerCombat;

    public override IEnumerator StartState()
    {
        yield return HouseKeeping();

        playerCombat = combatManager.playerCombat;
        yield return combatManager.PositionCombatant(playerCombat.gameObject, playerCombat.fightingPosition.transform.position);
        playerCombat.GetComponent<PlayerMovementScript>().lookDirection = combatManager.battleScheme.playerDefaultLookDirection;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", true);

        SetPlayerUI();
        yield return new WaitForSeconds(0.1f);

        foreach (Enemy enemy in combatManager.enemies)
        {
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.targetToAttack = combatManager.allAllies[Random.Range(0, combatManager.allAllies.Count)];
            SetDefaultLookDirectionAndType(enemy);
            SetcombatantUI(enemy);
            yield return new WaitForSeconds(0.1f);
        }

        foreach (Ally ally in combatManager.allies)
        {
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            ally.targetToAttack = combatManager.enemies[Random.Range(0, combatManager.enemies.Count)];
            SetDefaultLookDirectionAndType(ally);
            SetcombatantUI(ally);
            yield return new WaitForSeconds(0.1f);
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

        combatManager.SetState(combatManager.firstMove);
    }

    void SetcombatantUI(Enemy enemy)
    {
        GameObject newEnemyCombatUI = Instantiate(combatantUIPrefab, enemy.gameObject.transform);
        newEnemyCombatUI.transform.localPosition = Vector3.zero;
        newEnemyCombatUI.name = "combatantUI For " + enemy.combatantName;
        var enemycombatantUI = newEnemyCombatUI.GetComponent<EnemyUI>();
        enemy.combatantUI = enemycombatantUI;

        enemycombatantUI.partsTargetDisplay.enemy = enemy;
        enemycombatantUI.fendScript.combatManager = combatManager;
        enemycombatantUI.damageTakenDisplay.DisableDamageDisplay();
        enemycombatantUI.partsTargetDisplay.InitializeEnemyPartsHP();

        //flip UI elements based on look direction
        if (enemy.forceLookDirection == Vector2.right)
        {
            var flippedPos = enemy.combatantUI.attackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            enemycombatantUI.attackDisplay.transform.localPosition = flippedPos;
            enemycombatantUI.partsTargetDisplay.FlipTargetDisplay();
        }

        enemy.InitialiseCombatantStats();
        enemycombatantUI.statsDisplay.ShowStatsDisplay(true);
    }

    void SelectAndDisplayEnemyMove(Enemy enemy)
    {
        enemy.SelectMove();
        enemy.moveSelected.LoadMoveStats(enemy, combatManager);

        if (enemy.attackTotal > 0)
        {
            enemy.combatantUI.attackDisplay.UpdateAttackDisplay(enemy.attackTotal);
            enemy.combatantUI.attackDisplay.ShowAttackDisplay(true);
        }

        if (enemy.fendTotal > 0)
        {
            enemy.combatantUI.fendScript.ShowFendDisplay(true);
            enemy.combatantUI.fendScript.UpdateFendText(enemy.fendTotal);
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

    void SetcombatantUI(Ally ally)
    {
        ally.combatantUI.name = "combatantUI For " + ally.combatantName;
        ally.combatantUI = ally.combatantUI.GetComponent<CombatantUI>();
        ally.combatantUI.fendScript.combatManager = combatManager;
        ally.combatantUI.fendScript.UpdateFendText(ally.fendTotal);
        ally.combatantUI.damageTakenDisplay.DisableDamageDisplay();
        ally.InitialiseCombatantStats();
        ally.combatantUI.statsDisplay.ShowStatsDisplay(true);
    }

    void SelectAndDisplayAllyMove(Ally ally)
    {
        ally.SelectMove();
        ally.moveSelected.LoadMoveStats(ally, combatManager);

        if (ally.attackTotal > 0)
        {
            ally.combatantUI.attackDisplay.UpdateAttackDisplay(ally.AllyAttackTotal());
            ally.combatantUI.attackDisplay.ShowAttackDisplay(true);
        }

        if (ally.fendTotal > 0)
        {
            ally.combatantUI.fendScript.UpdateFendText(ally.fendTotal);
        }

        //flip UI elements based on look direction
        if (ally.forceLookDirection == Vector2.right)
        {
            var flippedPos = ally.combatantUI.attackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            ally.combatantUI.attackDisplay.transform.localPosition = flippedPos;
        }
    }

    IEnumerator HouseKeeping()
    {
        yield return new WaitForSeconds(0.01f);
        combatManager.playerCombat.fightingPosition = combatManager.battleScheme.playerFightingPosition;
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.isBattleMode = true;
        CombatEvents.LockPlayerMovement.Invoke();
        FieldEvents.isCameraFollow = false;
    }

    void SetPlayerUI()
    {
        playerCombat.combatantUI.fendScript.combatManager = combatManager;
        playerCombat.combatantUI.fendScript.ShowFendDisplay(false);
        playerCombat.InitialiseCombatantStats();
    }
}