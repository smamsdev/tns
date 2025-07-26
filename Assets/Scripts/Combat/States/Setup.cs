using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Setup : State
{
    [SerializeField] GameObject combatantUIPrefab;
    [SerializeField] CombatMenuManager combatMenuManager;
    PlayerCombat playerCombat;

    public override IEnumerator StartState()
    {
        yield return new WaitForSeconds(0.01f);
        combatManager.playerCombat.fightingPosition = combatManager.battleScheme.playerFightingPosition;
        CombatEvents.BattleMode?.Invoke(true);
        CombatEvents.isBattleMode = true;
        FieldEvents.isCameraFollow = false;

        playerCombat = combatManager.playerCombat;
        yield return combatManager.PositionCombatant(playerCombat.gameObject, playerCombat.fightingPosition.transform.position);

        PlayerMovementScript playerMovementScript = playerCombat.GetComponent<PlayerMovementScript>();
        playerMovementScript.lookDirection = combatManager.battleScheme.playerDefaultLookDirection;
        playerMovementScript.playerRigidBody2d.bodyType = RigidbodyType2D.Kinematic;

        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", true);

        SetPlayerUI();
        yield return new WaitForSeconds(0.1f);

        foreach (Enemy enemy in combatManager.enemies)
        {
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.targetToAttack = combatManager.allies[Random.Range(0, combatManager.allies.Count)];
            SetDefaultLookDirectionAndType(enemy);
            SetcombatantUI(enemy);
            yield return new WaitForSeconds(0.1f);
        }

        foreach (Ally ally in combatManager.allies)
        {
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            SetDefaultLookDirectionAndType(ally);
            SetcombatantUI(ally);

            //flip UI elements based on look direction
            if (ally.forceLookDirection == Vector2.left)
            {
                var flippedPos = ally.combatantUI.attackDisplay.transform.localPosition;
                flippedPos.x = -flippedPos.x;
                ally.combatantUI.attackDisplay.transform.localPosition = flippedPos;
            }

            yield return new WaitForSeconds(0.1f);
        }

        FieldEvents.isCameraFollow = true;

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectAndDisplayCombatantMove(ally);
            ally.targetToAttack = combatManager.enemies[Random.Range(0, combatManager.enemies.Count)];
        }

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectAndDisplayCombatantMove(enemy);
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

        enemycombatantUI.fendScript.combatManager = combatManager;

        //flip UI elements based on look direction
        if (enemy.forceLookDirection == Vector2.left)
        {
            var flippedPos = enemy.combatantUI.attackDisplay.transform.localPosition;
            flippedPos.x = -flippedPos.x;
            enemycombatantUI.attackDisplay.transform.localPosition = flippedPos;
        }

        enemy.InitialiseCombatantStats();
        enemycombatantUI.statsDisplay.ShowStatsDisplay(true);
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
        ally.combatantUI.fendScript.fendTextMeshProUGUI.text = ally.fendTotal.ToString();
        ally.InitialiseCombatantStats();
        ally.combatantUI.statsDisplay.ShowStatsDisplay(true);
    }

    void SetPlayerUI()
    {
        playerCombat.combatantUI.fendScript.combatManager = combatManager;
        playerCombat.InitialiseCombatantStats();
    }
}