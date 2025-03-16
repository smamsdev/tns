using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public State currentState;

    [Header("Settings")]

    public Battle battleScheme;

    public PlayerCombat playerCombat;
    public List<Ally> allies;
    public List<Enemy> enemies;

    public List<Combatant> allAllies;

    [Header("PlayerMoves")]
    public PlayerMove selectedPlayerMove;
    public int selectedEnemy = 0;

    [Header("Debugging")]

    public GameObject player;

    public CombatUIManager CombatUIManager;
    public PlayerMoveManager playerMoveManager;
    public int roundCount;

    [Header("States")]
    public Setup setup;
    public FirstMove firstMove;
    public SecondMove secondMove;
    public EnemySelect enemySelect;
    public AttackTarget attackTarget;
    public AllyMoveState allyMoveState;
    public ApplyPlayerMove applyMove;
    public EnemyMoveState enemyMoveState;
    public RoundReset roundReset;
    public Victory victory;
    public Defeat defeat;
    public GearSelect gearSelect;

    [Header("Movement")]

    public GameObject combatMovementPrefab;
    public CameraFollow cameraFollow;
    public Animator playerAnimator;

    [HideInInspector] public int enemyRawAttackPower;

    private void OnEnable()
    {
        CombatEvents.PlayerDefeated += PlayerDefeated;
    }

    private void OnDisable()
    {
        CombatEvents.PlayerDefeated -= PlayerDefeated;
    }

    public void StartBattle()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMoveManager = player.GetComponentInChildren<PlayerMoveManager>();
        playerCombat = player.GetComponentInChildren<PlayerCombat>();

        if (playerMoveManager == null)
        {
            Debug.LogError("PlayerMoveManager is not found in StartBattle");
            return;
        }

        if (battleScheme.allies != null)
        
        {
            allies = new List<Ally>(battleScheme.allies);
        }

        if (battleScheme.enemies != null)

        {
            enemies = new List<Enemy>(battleScheme.enemies);
        }

        else { Debug.Log("enemy not set in Battle"); }

        allAllies = new List<Combatant> { playerCombat };
        allAllies.AddRange(allies);

        SetState(setup);
    }

    public void SetState(State state)
    {
        currentState = state;
        StartCoroutine(currentState.StartState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.StateUpdate();
        }
    }

    public IEnumerator PositionCombatant(GameObject goToMove, Vector3 targetPosition, float stoppingPercentage = 100f, bool useTimeout = false)
    {
        var combatantMoveMentInstanceGO = Instantiate(combatMovementPrefab, this.transform);
        var combatantMoveMentInstance = combatantMoveMentInstanceGO.GetComponent<CombatMovement>();
        combatantMoveMentInstanceGO.name = "MoveCombatant" + goToMove.gameObject.name;

        yield return (combatantMoveMentInstance.MoveCombatant(goToMove, targetPosition, stoppingPercentage = 100f, useTimeout = false));
        Destroy(combatantMoveMentInstanceGO);
    }

    void PlayerDefeated()
    {
        defeat.playerDefeated = true;
        SetState(defeat);
    }
}