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
        }

        foreach (Ally ally in combatManager.allies)
        {
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
        }

        FieldEvents.isCameraFollow = true;

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
            combatManager.SetRigidBodyType(enemy, RigidbodyType2D.Kinematic);
            enemy.GetComponent<Animator>().SetBool("isCombat", true);
            SetcombatantUI(enemy);
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont
            combatManager.SelectAndDisplayCombatantMove(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectTargetToAttack(ally, combatManager.enemies);
            combatManager.SetRigidBodyType(ally, RigidbodyType2D.Kinematic);
            ally.GetComponent<Animator>().SetBool("isCombat", true);
            SetcombatantUI(ally);
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont
            combatManager.SelectAndDisplayCombatantMove(ally);
        }

        yield return new WaitForSeconds(1);
        combatManager.SetState(combatManager.firstMove);
    }

    void SetcombatantUI(Enemy enemy)
    {
        GameObject newEnemyCombatUI = Instantiate(combatantUIPrefab, enemy.gameObject.transform);
        newEnemyCombatUI.transform.localPosition = Vector3.zero;
        newEnemyCombatUI.name = "combatantUI For " + enemy.combatantName;
        var enemycombatantUI = newEnemyCombatUI.GetComponent<CombatantUI>();
        enemy.combatantUI = enemycombatantUI;
        enemycombatantUI.fendScript.combatManager = combatManager;

        //flip UI elements based on look direction
        if (enemy.GetComponent<MovementScript>().lookDirection == Vector2.left)
        {
           combatManager.SetUIBasedOnLookDir(enemy);
        }

        enemy.InitialiseCombatantStats();
        enemycombatantUI.statsDisplay.ShowStatsDisplay(true);
    }

    void SetcombatantUI(Ally ally)
    {
        ally.combatantUI.name = "combatantUI For " + ally.combatantName;
        ally.combatantUI = ally.combatantUI.GetComponent<CombatantUI>();
        ally.combatantUI.fendScript.combatManager = combatManager;
        ally.combatantUI.fendScript.fendTextMeshProUGUI.text = ally.fendTotal.ToString();

        if (ally.GetComponent<MovementScript>().lookDirection == Vector2.left)
        {
            combatManager.SetUIBasedOnLookDir(ally);
        }

        ally.InitialiseCombatantStats();
        ally.combatantUI.statsDisplay.ShowStatsDisplay(true);
    }

    void SetPlayerUI()
    {
        playerCombat.combatantUI.fendScript.combatManager = combatManager;
        playerCombat.InitialiseCombatantStats();
    }
}