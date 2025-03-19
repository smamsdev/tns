using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] CombatManager combatManager;
    [SerializeField] GameObject combatUIContainer;
    [SerializeField] GameObject playerStatsUIContainer;
    [SerializeField] GameObject combatantUIPrefab;
    [SerializeField] GameObject combatantUI;
    [SerializeField] GameObject playerFendContainerPrefab;

    public override IEnumerator StartState()

    {
        yield return HouseKeeping();

        PlayerCombat player = combatManager.playerCombat;
        yield return combatManager.PositionCombatant(player.gameObject, player.fightingPosition.transform.position);
        player.GetComponent<PlayerMovementScript>().lookDirection = combatManager.battleScheme.playerDefaultLookDirection;
        var playerAnimator = player.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", true);

        yield return SetPlayerUI();
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

            enemycombatantUI.partsTargetDisplay.enemy = enemy;
            enemycombatantUI.partsTargetDisplay.combatManager = combatManager;
            enemycombatantUI.fendScript.combatManager = combatManager;
            enemycombatantUI.damageTakenDisplay.DisableDamageDisplay();
            enemycombatantUI.statsDisplay.InitializeStatsUI(enemy);
            enemycombatantUI.partsTargetDisplay.InitializeEnemyPartsHP();
            enemycombatantUI.statsDisplay.ShowStatsDisplay(true);

        //flip UI elements based on look direction
        if (enemy.forceLookDirection == Vector2.right)
        {
            var flippedPos = enemy.combatantUI.attackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            enemycombatantUI.attackDisplay.transform.localPosition = flippedPos;
            enemycombatantUI.partsTargetDisplay.FlipTargetDisplay();
        }
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
            combatantUI.name = "combatantUI For " + ally.combatantName;
            ally.combatantUI = combatantUI.GetComponent<CombatantUI>();
            ally.combatantUI.fendScript.combatManager = combatManager;
            ally.combatantUI.fendScript.UpdateFendText(ally.fendTotal);
            ally.combatantUI.statsDisplay.InitializeStatsUI(ally);
            ally.combatantUI.statsDisplay.ShowStatsDisplay(false);
            ally.combatantUI.damageTakenDisplay.DisableDamageDisplay();

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
        yield break;
    }
}