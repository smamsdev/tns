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
        CombatEvents.isBattleMode = true;
        combatManager.cameraFollow.transformToFollow = combatManager.battleScheme.battleCenterPosition;


        //position player
        playerCombat = combatManager.playerCombat;
        MovementScript playerMovementScript = playerCombat.movementScript;
        playerMovementScript.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
        yield return combatManager.PositionCombatant(playerCombat.gameObject, playerCombat.fightingPosition.transform.position);
        playerMovementScript.movementSpeed = playerMovementScript.defaultMovementspeed * 1;

        //set up player stance and UI
        playerMovementScript.lookDirection = combatManager.battleScheme.playerDefaultLookDirection;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", true);
        SetPlayerUI();

        yield return new WaitForSeconds(0.1f);

        //position enemies
        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SetRigidBodyType(enemy, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.movementScript.movementSpeed = enemy.movementScript.defaultMovementspeed * 1;
        }

        //position allies
        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SetRigidBodyType(ally, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            ally.movementScript.movementSpeed = ally.movementScript.defaultMovementspeed * 1;
        }

        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
            enemy.GetComponent<Animator>().SetBool("isCombat", true);
            SetcombatantUI(enemy);
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont
            combatManager.SelectAndDisplayCombatantMove(enemy);
        }

        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SelectTargetToAttack(ally, combatManager.enemies);
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