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
        playerCombat = combatManager.playerCombat;

        //position player
        MovementScript playerMovementScript = playerCombat.movementScript;
        playerMovementScript.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
        yield return combatManager.PositionCombatant(playerCombat.gameObject, playerCombat.fightingPosition.transform.position);
        playerMovementScript.movementSpeed = playerMovementScript.defaultMovementspeed * 1;

        //set up player stance and UI
        playerMovementScript.lookDirection = combatManager.battleScheme.playerDefaultLookDirection;
        var playerAnimator = playerCombat.GetComponent<Animator>();
        playerAnimator.SetBool("isCombat", true);
        SetPlayerUI();

        //Set combat animations
        foreach (Combatant combatant in combatManager.enemies)
        {
            combatant.GetComponent<Animator>().SetBool("isCombat", true);
        }

        foreach (Combatant combatant in combatManager.allies)
        {
            combatant.GetComponent<Animator>().SetBool("isCombat", true);
        }

        yield return new WaitForSeconds(0.1f);

        //position allies
        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SetRigidBodyType(ally, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            ally.movementScript.movementSpeed = ally.movementScript.defaultMovementspeed * 1;
        }

        //position enemies
        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SetRigidBodyType(enemy, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.movementScript.movementSpeed = enemy.movementScript.defaultMovementspeed * 1;
        }

        //select enemy attack
        foreach (Enemy enemy in combatManager.enemies)
        {
            SetcombatantUI(enemy);
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont

            if (!combatManager.battleScheme.isEnemyFlanked)
            {
                combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
                combatManager.SelectAndDisplayCombatantMove(enemy);
                yield return new WaitForSeconds(.7f);
                enemy.combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeDown");
            }

            if (enemy.fendTotal > 0)
            {
                enemy.combatantUI.fendScript.fendAnimator.Play("FendFade");
            }

        }

        //select ally attack
        foreach (Ally ally in combatManager.allies)
        {
            SetcombatantUI(ally);
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont

            if (!combatManager.battleScheme.isAllyFlanked)
            {
                combatManager.SelectTargetToAttack(ally, combatManager.enemies);
                combatManager.SelectAndDisplayCombatantMove(ally);
                yield return new WaitForSeconds(.7f);
                ally.combatantUI.attackDisplay.attackDisplayAnimator.Play("CombatantAttackDamageFadeDown");
            }

            if (ally.fendTotal > 0)
            {
                ally.combatantUI.fendScript.fendAnimator.Play("FendFade");
            }
        }

        yield return new WaitForSeconds(1);
        combatManager.SetState(combatManager.firstMove);
    }

    void SetcombatantUI(Combatant combatant)
    {
        GameObject newCombatantUIGO = Instantiate(combatantUIPrefab, combatant.gameObject.transform);
        newCombatantUIGO.transform.localPosition = Vector3.zero;
        newCombatantUIGO.name = "combatantUI For " + combatant.combatantName;

        var combatantUI = newCombatantUIGO.GetComponent<CombatantUI>();
        combatant.combatantUI = combatantUI;
        combatantUI.combatUIContainer.SetActive(true);
        combatantUI.fendScript.combatManager = combatManager;

        //flip UI elements based on look direction
        if (combatant.GetComponent<MovementScript>().lookDirection == Vector2.left)
        {
            combatManager.SetUIBasedOnLookDir(combatant);
        }

        combatant.InitialiseCombatantStats();
        combatantUI.statsDisplay.InitialiseCombatStatsDisplay(combatant);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
    }

    void SetPlayerUI()
    {
        playerCombat.combatantUI.combatUIContainer.SetActive(true);
        playerCombat.combatantUI.fendScript.combatManager = combatManager;
        playerCombat.InitialiseCombatantStats();
    }
}