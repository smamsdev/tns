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
        playerAnimator.SetTrigger("CombatIdle");
        InitializePermanentStatsAndGear();
        playerCombat.playerMoveManager.InstantiateAllEquippedMoves();
        SetPlayerUI();

        yield return new WaitForSeconds(0.1f);

        //position allies
        foreach (Ally ally in combatManager.allies)
        {
            combatManager.SetRigidBodyType(ally, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(ally.gameObject, ally.fightingPosition.transform.position);
            ally.movementScript.movementSpeed = ally.movementScript.defaultMovementspeed * 1;
            ally.movementScript.animator.SetTrigger("CombatIdle");
        }

        //position enemies
        foreach (Enemy enemy in combatManager.enemies)
        {
            combatManager.SetRigidBodyType(enemy, RigidbodyType2D.Kinematic);
            yield return combatManager.PositionCombatant(enemy.gameObject, enemy.fightingPosition.transform.position);
            enemy.movementScript.movementSpeed = enemy.movementScript.defaultMovementspeed * 1;
            enemy.movementScript.animator.SetTrigger("CombatIdle");
        }

        //set enemy ui and attack
        foreach (Enemy enemy in combatManager.enemies)
        {
            SetcombatantUI(enemy);
            enemy.InstantiateMoves();
            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont

            if (!combatManager.battleScheme.isEnemyFlanked)
            {
                combatManager.SelectTargetToAttack(enemy, combatManager.allAlliesToTarget);
                combatManager.SelectCombatantMove(enemy);
                combatManager.cameraFollow.transformToFollow = enemy.transform;
                enemy.combatantUI.DisplayCombatantMove(enemy);
                yield return new WaitForSeconds(1f);
                enemy.combatantUI.attackDisplay.ShowAttackDisplay(enemy, false);
                enemy.combatantUI.fendScript.ShowFendDisplay(enemy, false);
            }
        }

        //set ally ui and attack
        foreach (Ally ally in combatManager.allies)
        {
            SetcombatantUI(ally);
            ally.InstantiateMoves();

            if (ally is PartyMemberCombat)
            {
                PartyMemberCombat partyMember = ally as PartyMemberCombat;

                partyMember.AttackBase = partyMember.partyMemberSO.AttackBase;
                partyMember.FendBase = partyMember.partyMemberSO.FendBase;
                partyMember.MaxHP = partyMember.partyMemberSO.MaxHP;
                partyMember.CurrentHP = partyMember.partyMemberSO.CurrentHP;
            }

            yield return new WaitForSeconds(0.1f); //i cant remember why u have to wait but attack ui wont appear if you dont

            if (!combatManager.battleScheme.isAllyFlanked)
            {
                combatManager.SelectTargetToAttack(ally, combatManager.enemies);
                combatManager.SelectCombatantMove(ally);
                combatManager.cameraFollow.transformToFollow = ally.transform;
                ally.combatantUI.DisplayCombatantMove(ally);
                yield return new WaitForSeconds(1f);
                ally.combatantUI.attackDisplay.ShowAttackDisplay(ally, false);
                ally.combatantUI.fendScript.ShowFendDisplay(ally, false);
            }
        }

        //yield return new WaitForSeconds(1);
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
        combatant.combatantUI.attackDisplay.SetAttackDisplayDirBasedOnLookDir(combatant);
        combatantUI.statsDisplay.InitialiseCombatStatsDisplay(combatant);
        combatantUI.statsDisplay.ShowStatsDisplay(false);
    }

    void SetPlayerUI()
    {
        playerCombat.combatantUI.statsDisplay.InitialiseCombatStatsDisplay(playerCombat);
        playerCombat.combatantUI.combatUIContainer.SetActive(true);
        playerCombat.combatantUI.fendScript.combatManager = combatManager;
    }

    void InitializePermanentStatsAndGear()
    {
        playerCombat.playerInventory.InstantiateAllEquippedGear(combatManager);

        playerCombat.MaxHP = playerCombat.playerPermanentStats.maxHP;
        playerCombat.CurrentHP = playerCombat.playerPermanentStats.currentHP;
        playerCombat.MaxPotential = playerCombat.playerPermanentStats.maxPotential;
        playerCombat.CurrentPotential = playerCombat.playerPermanentStats.currentPotential;
        playerCombat.AttackBase = playerCombat.playerPermanentStats.attackBase;
        playerCombat.FendBase = playerCombat.playerPermanentStats.fendBase;
        playerCombat.focusBase = playerCombat.playerPermanentStats.focusBase;
    }
}